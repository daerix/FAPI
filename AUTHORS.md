## Author
* Tom Ruscelli ([iTyom](https://github.com/iTyom))
* Maxime Abdelouahed-Robeque ([MaximeRA](https://github.com/MaximeRA))
* Ludovic Robez ([LudovicRobez](https://github.com/LudovicRobez))
* Virgile Sassano ([daerix](https://github.com/daerix))

## Responsabilité
### Tom Ruscelli
* Réalisation de la librairie "[Authentification.API](https://github.com/daerix/FAPI/tree/master/src/Services/Authentification/Authentification.API)"
* Réalisation des Tests Fonctionnels "[IQueryableExtentions](https://github.com/daerix/FAPI/tree/master/src/Services/Authentification/Authentification.Test)"
* Intégration Authentification.API dans "[Catalog.API](https://github.com/daerix/FAPI/tree/master/src/Services/Catalog/Catalog.API)" et "[Basket.API](https://github.com/daerix/FAPI/tree/master/src/Services/Basket/Basket.API)"

### Maxime Abdelouahed-Robeque
* Réalisation de la librairie "[Catalog.API](https://github.com/daerix/FAPI/tree/master/src/Services/Catalog/Catalog.API)"
* Réalisation des Test unitaires sur la classe "[DbContext](https://github.com/daerix/FAPI/blob/master/src/Core/ApiLibrary.Test/BaseDbContextTest.cs)"
* Peer Programming sur les Tests Unitaires de "[Api.Core](https://github.com/daerix/FAPI/blob/master/src/Core/ApiLibrary.Test)"
* Correction de Bugs dans "[ApiLibrary](https://github.com/daerix/FAPI/tree/master/src/Core/ApiLibrary)"
* Création du diaporama

### Ludovic Robez
* Réalisation de la librairie "[ApiLibrary](https://github.com/daerix/FAPI/tree/master/src/Core/ApiLibrary)"
* Réalisation des Tests Unitaires "[IQueryableExtentions](https://github.com/daerix/FAPI/blob/master/src/Core/ApiLibrary.Test/IQueryableExtensionsTest.cs)"
* Réalisation des Tests Unitaires "[ExpressionExtentions](https://github.com/daerix/FAPI/blob/master/src/Core/ApiLibrary.Test/ExpressionExtensionsTest.cs)" 
* Réalisation des Tests Unitaires "[QueryParams](https://github.com/daerix/FAPI/blob/master/src/Core/ApiLibrary.Test/QueryParamsTest.cs)"
* Réalisation des Tests Fonctionnels "[BaseController](https://github.com/daerix/FAPI/blob/master/src/Core/ApiLibrary.Test/BaseControllerTest.cs)"

### Virgile Sassano
* Réalisation du projet "[Basket.API](https://github.com/daerix/FAPI/tree/master/src/Services/Basket/Basket.API)"
* Réalisation des Tests Fonctionnels "[Basket.Test](https://github.com/daerix/FAPI/tree/master/src/Services/Basket/Basket.Test)"
* Création de la base de donnée "[BasketDB](https://portal.azure.com/#@ynov.com/resource/subscriptions/3ba14322-177a-4733-bf54-2676d79bda07/resourceGroups/FAPI/providers/Microsoft.Sql/servers/fapi-basket-server/databases/BasketDB/overview)"
* Création du "[CI](https://dev.azure.com/virgilesassano/virgilesassano/_build?definitionId=4&_a=summary)"
* Création des Dockerfiles "[Basket Dockerfile](https://github.com/daerix/FAPI/tree/master/src/Services/Basket/Basket.API/Dockerfile)" "[Catalog Dockerfile](https://github.com/daerix/FAPI/tree/master/src/Services/Catalog/Catalog.API/Dockerfile)" "[Authentification Dockerfile](https://github.com/daerix/FAPI/tree/master/src/Services/Authentification/Authentification.API/Dockerfile)"
* Création des fichiers de configuration YML pour k8s "[Basket Deployment](https://github.com/daerix/FAPI/tree/master/src/Services/Basket/Basket.API/basket-api.yml)" "[Catalog Deployment](https://github.com/daerix/FAPI/tree/master/src/Services/Catalog/Catalog.API/catalog-api.yml)" "[Authentification Deployment](https://github.com/daerix/FAPI/tree/master/src/Services/Authentification/Authentification.API/authentification-api.yml)" "[FAPI Ingress](https://github.com/daerix/FAPI/tree/master/src/fapi-ingress.yml)"
