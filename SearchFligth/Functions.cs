using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SearchFligth.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SearchFligth
{
    internal class Functions
    {
        protected readonly ILogger Logger;

        /// <summary>
        /// Retrieves a list of disponible flight
        /// </summary>
        /// <param name="_aeroOri">Origin airport </param>
        /// <param name="_aeroDes">Destiny airport </param>
        /// <param name="_data">Data of Flight </param>
        /// <param name="listNine">"List of 99planes flight </param>
        /// <param name="listUberAir">List of UberAir flight </param>
        /// <returns>Return with List of disponible flight</returns>
        internal List<FlightList> ReturnFlightList(string _aeroOri, string _aeroDes, DateTime _data, List<NineNinePlanes> listNine, List<UberAir> listUberAir)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(ReturnFlightList));

            List<FlightList> FlightList = new List<FlightList>();

            List<NineNinePlanes> FiltNineDest = new List<NineNinePlanes>();
            List<UberAir> FiltUberDest = new List<UberAir>();
            List<Trechos> listTrechos;

            try
            {

                foreach (NineNinePlanes NinePlane in listNine)
                {
                    if (NinePlane.destino.ToString().ToUpper() == _aeroDes.ToUpper() &&
                        NinePlane.data_saida >= _data)
                        FiltNineDest.Add(NinePlane);
                }

                foreach (UberAir UberPlane in listUberAir)
                {
                    if (UberPlane.aeroporto_destino.ToString().ToUpper() == _aeroDes.ToUpper() &&
                        UberPlane.data >= _data)
                        FiltUberDest.Add(UberPlane);
                }

                Trechos trechos;
                foreach (NineNinePlanes NineFlightD in FiltNineDest)
                {

                    if (NineFlightD.origem == _aeroOri && NineFlightD.destino == _aeroDes)
                    {

                        listTrechos = FillStretch99Planes(null, null, NineFlightD);

                        FlightList.Add(FillFlightList(_aeroOri, _aeroDes, listTrechos));

                        continue;
                    }

                    foreach (NineNinePlanes Route1 in listNine)
                    {
                        trechos = new Trechos();
                        listTrechos = new List<Trechos>();
                        bool validate = false;
                        if (Route1.destino == NineFlightD.origem &&
                            NineFlightD.data_saida.AddTicks(NineFlightD.saida.Ticks) >=
                            Route1.data_saida.AddTicks(Route1.chegada.Ticks))
                        {

                            if (Route1.origem == _aeroOri && Route1.destino == NineFlightD.origem &&
                                (NineFlightD.data_saida.AddTicks(NineFlightD.saida.Ticks) -
                                 Route1.data_saida.AddTicks(Route1.chegada.Ticks)).TotalHours < 12)
                            {

                                listTrechos = FillStretch99Planes(null, Route1, NineFlightD);
                                validate = true;
                            }

                            if (!validate)
                            {
                                foreach (NineNinePlanes Route2 in listNine)
                                {
                                    if (Route2.origem == _aeroOri && Route2.destino == Route1.origem &&
                                        Route1.data_saida.AddTicks(Route1.saida.Ticks) >=
                                        Route2.data_saida.AddTicks(Route2.chegada.Ticks))
                                    {

                                        if ((Route1.data_saida.AddTicks(Route1.saida.Ticks) -
                                        Route2.data_saida.AddTicks(Route2.chegada.Ticks)).TotalHours < 12 &&
                                        (NineFlightD.data_saida.AddTicks(NineFlightD.saida.Ticks) -
                                        Route1.data_saida.AddTicks(Route1.chegada.Ticks)).TotalHours < 12)
                                        {
                                            listTrechos = FillStretch99Planes(Route2, Route1, NineFlightD);

                                            validate = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (validate)
                        {
                            FlightList.Add(FillFlightList(_aeroOri, _aeroDes, listTrechos));
                        }
                    }
                }

                foreach (UberAir UberFlightD in FiltUberDest)
                {

                    if (UberFlightD.aeroporto_origem == _aeroOri && UberFlightD.aeroporto_destino == _aeroDes)
                    {

                        listTrechos = FillStretchUberPlanes(null, null, UberFlightD);

                        FlightList.Add(FillFlightList(_aeroOri, _aeroDes, listTrechos));

                        continue;
                    }

                    foreach (UberAir Route1 in listUberAir)
                    {
                        trechos = new Trechos();
                        listTrechos = new List<Trechos>();
                        bool validate = false;
                        if (Route1.aeroporto_destino == UberFlightD.aeroporto_origem &&
                            UberFlightD.data.AddTicks(UberFlightD.horario_saida.Ticks) >=
                            Route1.data.AddTicks(Route1.horario_chegada.Ticks))
                        {

                            if (Route1.aeroporto_origem == _aeroOri && Route1.aeroporto_destino == UberFlightD.aeroporto_origem &&
                                (UberFlightD.data.AddTicks(UberFlightD.horario_saida.Ticks) -
                                 Route1.data.AddTicks(Route1.horario_chegada.Ticks)).TotalHours < 12)
                            {

                                listTrechos = FillStretchUberPlanes(null, Route1, UberFlightD);
                                validate = true;
                            }

                            if (!validate)
                            {
                                foreach (UberAir Route2 in listUberAir)
                                {
                                    if (Route2.aeroporto_origem == _aeroOri && Route2.aeroporto_destino == Route1.aeroporto_origem &&
                                        Route1.data.AddTicks(Route1.horario_saida.Ticks) >=
                                        Route2.data.AddTicks(Route2.horario_chegada.Ticks))
                                    {

                                        if ((Route1.data.AddTicks(Route1.horario_saida.Ticks) -
                                        Route2.data.AddTicks(Route2.horario_chegada.Ticks)).TotalHours < 12 &&
                                        (UberFlightD.data.AddTicks(UberFlightD.horario_saida.Ticks) -
                                        Route1.data.AddTicks(Route1.horario_chegada.Ticks)).TotalHours < 12)
                                        {
                                            listTrechos = FillStretchUberPlanes(Route2, Route1, UberFlightD);

                                            validate = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (validate)
                        {
                            FlightList.Add(FillFlightList(_aeroOri, _aeroDes, listTrechos));
                        }
                    }
                }

                return FlightList;
            }
            catch (Exception ex)
            {
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(ReturnFlightList), ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of AirPort
        /// </summary>
        /// <returns>Return with List of AirPort</returns>
        internal List<AirPort> AriPortsList()
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(AriPortsList));

            StreamReader sReader;
            string json;

            List<AirPort> ListAirPorts;

            try
            {
                sReader = new StreamReader(".\\Files\\aeroportos.json");
                json = sReader.ReadToEnd();

                sReader.Close();
                sReader.Dispose();
                ListAirPorts = new List<AirPort>();
                ListAirPorts = JsonConvert.DeserializeObject<List<AirPort>>(json);
                return ListAirPorts;
            }
            catch (Exception ex)
            {
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(AriPortsList), ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of NineNinePlanes
        /// </summary>
        /// <returns>Return with List of NineNinePlanes</returns>
        internal List<NineNinePlanes> ReadJson()
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(ReadJson));

            StreamReader sReader;
            string json;

            List<NineNinePlanes> NinePlanes;

            try
            {
                sReader = new StreamReader(".\\Files\\99planes.json");
                json = sReader.ReadToEnd();

                sReader.Close();
                sReader.Dispose();

                NinePlanes = new List<NineNinePlanes>();

                NinePlanes = JsonConvert.DeserializeObject<List<NineNinePlanes>>(json);

                return NinePlanes;
            }
            catch (Exception ex)
            {
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(ReadJson), ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of flight Trechos
        /// </summary>
        /// <param name="route2">class UberAir</param>
        /// <param name="route1">class UberAir </param>
        /// <param name="uberFlightD">class UberAir </param>
        /// <returns>Return with List of flight Trechos</returns>
        internal List<Trechos> FillStretchUberPlanes(UberAir route2, UberAir route1, UberAir uberFlightD)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(FillStretchUberPlanes));

            Trechos trechos;
            List<Trechos> listTrechos;

            try
            {
                listTrechos = new List<Trechos>();
                if (route2 != null)
                {
                    trechos = new Trechos();
                    trechos.origem = route2.aeroporto_origem;
                    trechos.destino = route2.aeroporto_destino;
                    trechos.saida = route2.data.AddTicks(route2.horario_saida.Ticks);
                    trechos.chegada = route2.data.AddTicks(route2.horario_chegada.Ticks);
                    trechos.operadora = "UberAir";
                    trechos.preco = route2.preco;
                    listTrechos.Add(trechos);
                }

                if (route1 != null)
                {
                    trechos = new Trechos();
                    trechos.origem = route1.aeroporto_origem;
                    trechos.destino = route1.aeroporto_destino;
                    trechos.saida = route1.data.AddTicks(route1.horario_saida.Ticks);
                    trechos.chegada = route1.data.AddTicks(route1.horario_chegada.Ticks);
                    trechos.operadora = "UberAir";
                    trechos.preco = route1.preco;
                    listTrechos.Add(trechos);
                }

                if (uberFlightD != null)
                {
                    trechos = new Trechos();
                    trechos.origem = uberFlightD.aeroporto_origem;
                    trechos.destino = uberFlightD.aeroporto_destino;
                    trechos.saida = uberFlightD.data.AddTicks(uberFlightD.horario_saida.Ticks);
                    trechos.chegada = uberFlightD.data.AddTicks(uberFlightD.horario_chegada.Ticks);
                    trechos.operadora = "UberAir";
                    trechos.preco = uberFlightD.preco;
                    listTrechos.Add(trechos);
                }

                return listTrechos;
            }
            catch (Exception ex)
            {
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(FillStretchUberPlanes), ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of FlightList
        /// </summary>
        /// <param name="_aeroOri">Origin airport </param>
        /// <param name="_aeroDes">Destiny airport </param>
        /// <param name="listTrechos">List of Trechos </param>
        /// <returns>Return with List of flight Trechos</returns>
        internal FlightList FillFlightList(string _aeroOri, string _aeroDes, List<Trechos> listTrechos)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(FillFlightList));
            FlightList UniFlightList;
            try
            {

                UniFlightList = new FlightList();
                UniFlightList.origem = _aeroOri;
                UniFlightList.destino = _aeroDes;
                UniFlightList.saida = listTrechos[0].saida.ToString();
                UniFlightList.chegada = listTrechos[listTrechos.Count - 1].chegada.ToString();
                UniFlightList.trechos = listTrechos;

                return UniFlightList;
            }
            catch (Exception ex)
            {
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(FillFlightList), ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of flight Trechos
        /// </summary>
        /// <param name="route2">class NineNinePlanes</param>
        /// <param name="route1">class NineNinePlanes </param>
        /// <param name="nineFlightD">class NineNinePlanes </param>
        /// <returns>Return with List of flight Trechos</returns>
        internal List<Trechos> FillStretch99Planes(NineNinePlanes route2 = null, NineNinePlanes route1 = null, NineNinePlanes nineFlightD = null)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(FillStretch99Planes));

            Trechos trechos;
            List<Trechos> listTrechos;

            try
            {
                listTrechos = new List<Trechos>();
                if (route2 != null)
                {
                    trechos = new Trechos();
                    trechos.origem = route2.origem;
                    trechos.destino = route2.destino;
                    trechos.saida = route2.data_saida.AddTicks(route2.saida.Ticks);
                    trechos.chegada = route2.data_saida.AddTicks(route2.chegada.Ticks);
                    trechos.operadora = "99Planes";
                    trechos.preco = route2.valor;
                    listTrechos.Add(trechos);
                }

                if (route1 != null)
                {
                    trechos = new Trechos();
                    trechos.origem = route1.origem;
                    trechos.destino = route1.destino;
                    trechos.saida = route1.data_saida.AddTicks(route1.saida.Ticks);
                    trechos.chegada = route1.data_saida.AddTicks(route1.chegada.Ticks);
                    trechos.operadora = "99Planes";
                    trechos.preco = route1.valor;
                    listTrechos.Add(trechos);
                }

                if (nineFlightD != null)
                {
                    trechos = new Trechos();
                    trechos.origem = nineFlightD.origem;
                    trechos.destino = nineFlightD.destino;
                    trechos.saida = nineFlightD.data_saida.AddTicks(nineFlightD.saida.Ticks);
                    trechos.chegada = nineFlightD.data_saida.AddTicks(nineFlightD.chegada.Ticks);
                    trechos.operadora = "99Planes";
                    trechos.preco = nineFlightD.valor;
                    listTrechos.Add(trechos);
                }

                return listTrechos;
            }
            catch (Exception ex)
            {
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(FillStretch99Planes), ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a list of UberAir flight
        /// </summary>
        /// <returns>Return with List of UberAir Flights</returns>
        internal List<UberAir> ReadUberAir()
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(ReadUberAir));

            List<UberAir> ListUberAir = new List<UberAir>();

            try
            {
                //read csv file
                var lines = System.IO.File.ReadAllLines(@".\\Files\\uberair.csv");
                int count = 0;

                //get line by line
                foreach (var line in lines)
                {
                    //put line in and array
                    string[] words = line.Split(',');

                    //jump frist line name of collumn
                    if (count == 0)
                    {
                        count++;
                        continue;
                    }

                    UberAir uberItem = new UberAir();

                    uberItem.numero_voo = words[0].ToString();
                    uberItem.aeroporto_origem = words[1].ToString();
                    uberItem.aeroporto_destino = words[2].ToString();
                    uberItem.data = Convert.ToDateTime(words[3].ToString());
                    uberItem.horario_saida = TimeSpan.Parse(words[4].ToString());
                    uberItem.horario_chegada = TimeSpan.Parse(words[5].ToString());
                    uberItem.preco = Decimal.Parse(words[6]);

                    ListUberAir.Add(uberItem);

                    count++;
                }

                return ListUberAir;
            }
            catch (Exception ex)
            {
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(ReadUberAir), ex.Message);
                return null;
            }
        }
    }
}
