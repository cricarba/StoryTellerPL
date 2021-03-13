# Story Teller PL [![Build Status](https://dev.azure.com/cricarbah/CriCarBa/_apis/build/status/Story%20Teller%20Master?branchName=master)](https://dev.azure.com/cricarbah/CriCarBa/_build/latest?definitionId=4&branchName=master)

Hola!! :soccer:

Cree este proyecto en son de juego. Que hace este proyecto?

Este proyecto hace webscrapy a la pagina de la premier League, de ella saca el minuto a minuto de los partido que se estan jugando y luego hace un tweet en la cuenta  [PremierSummary](https://twitter.com/PremierSummary) con cada registro.

Cuenta con scheduler para poder programar los partidos y que se ejecte de manera autonoma.

---

En este proyecto podras encontrar lo siguiente:

- Escrito en C#
- WebScrapy con sellenium. 
- Azure Key Vault
- Hingfire Scheduler

Crear varible de entorno para saber de donde tomar los datos de configuración

- SECRETS_LOCAL = indica si las variables de configuración se toma de las variables de entonrno locales o de Azure Secrets, True para variables locales

Variables de entorno o Azure Secrets

- hangFireDb = cadena de conexión para la base de datos del Scheduler
- chromeDriver= ruta del driver de sellenium
- oauthConsumerKey = Consumer Key de la cuenta de twitter
- oauthConsumerSecret = Consumer Secret de la ceunta de twitter
- oauthToken = token para autenticación de ña cuenta de twitter
- oauthTokenSecret = secret token de la cuenta de twiiter




