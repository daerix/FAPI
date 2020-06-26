# FAPI README

## Projets 

L'application est constitué de 3 Projets : Basket, Catalog, Authentification.
Toutes trois utilisant la library ApiCore mettant à disposition du CRUD générique.

## BDD 

Les bases de données de ces projets sont héberger sur des serveurs distrincts sur Azure.

## CI 

Afin d'avoir des images à jours pour notre architecture Kubernetes, un pipeline d'intégration 
continue est mis en place qui se chargera de lancer les jeux de tests unitaires puis de build les 
images docker et les envoyer sur un registry Azure Container Registry.

## Kubernetes

Malheureusement, Azure ne propose pas d'utilisation gratuite pour Kubernetes. Nous avons donc utilisé Docker-Desktop 
incluant Kubernetes afin de lancer notre architecture. 

Chaque projet contient un fichier '<nom-du-projet>-api.yml',
en exécutant la commande `kubectl apply -f <nom-du-projet>-api.yml` un deployment de 3 PODS se fera ainsi qu'un service
lié à ce projets.

Une fois les services et les deployment dans l'état 'running'. Lancez de la même façon le fichier fapi-ingress.yml situé dans
/src à la racine du projet.

Tout les projets seront donc accessible à l'adresse `localhost/<nom-du-projet>/api/v1/<route>`