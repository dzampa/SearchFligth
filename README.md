# API .net Core
Olá esse projeto mosta uma simples API desenvolvida em .net Core

---
## Framewors e Package
Microsoft.AspNetCore.App
Microsoft.NETCore.app
Json.Net
Newtonsoft.Json

## Etapas
Baixar o projeto e execultar o publish
Folder
Create Profile
Publish
#executando
Ir até a pasta de publish 
execultar o arquivo SearchFligth.exe
Aguadar a subida do servidor
## Teste API 
"applicationUrl": "http://localhost:5000",
"sslPort": "http://localhost:5001"

**Lista Aeroportos**
api/v1/AirPort/airport
Retorno JSON


**Exemplo de Response:**

```json
[
	{
		"nome":"Aeroporto Internacional Juscelino Kubitschek",
		"aeroporto":"BSB",
		"cidade":"Brasília"
	}
]
```
**Lista Voos disponiveis**
api/v1/AirPort/fligths?AeroOri=FLN&AeroDes=MCZ&Data=2019-02-10
ONDE Obrigatorio:
AeroOri = Aeroporto de origem EX: FLN
AeroDes = Aeroporto de destino EX: MCZ
Data = Data de viagem entre 2019-02-10 e 2019-02-18

**Exemplo de Response:**

```json
[
	{
		"origem": "GRU",
		"destino": "LOA",
		"saida": "YYYY-MM-DDTHH:mm:ss.sssZ",
		"chegada": "YYYY-MM-DDTHH:mm:ss.sssZ",
		"trechos": [
			{
				"origem": "GRU",
				"destino": "NYC",
				"saida": "YYYY-MM-DDTHH:mm:ss.sssZ",
				"chegada": "YYYY-MM-DDTHH:mm:ss.sssZ",
				"operadora": "UberAir",
				"preco": 1400.00
			},
			{
				"origem": "NYC",
				"destino": "LOA",
				"saida": "YYYY-MM-DDTHH:mm:ss.sssZ",
				"chegada": "YYYY-MM-DDTHH:mm:ss.sssZ",
				"operadora": "UberAir",
				"preco": 350.00
			}
		]
	}
]
```