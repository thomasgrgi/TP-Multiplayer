# TP Multiplayer - Guide de Lancement

## Prérequis
- Unity (version utilisée 6000.3.6f1)
- Casque VR 
- Connexion WiFi partagée entre l'ordinateur et le casque VR((de préférence)

## Étapes de Lancement

### 1. Configuration Réseau
- Connectez l'ordinateur et le casque VR au même réseau WiFi
- Dans Unity, ouvrez les scènes VRScene et SampleScene
- Dans le NetworkManager de chaque scène , champ "Address" du Unity Transport, entrez l'adresse IP du WiFi sur lequel sont connectés le PC et le casque

### 2. Build et Run
- Build et lancez le projet Unity

### 3. Connexion VR
- Une fois que l'utilisateur VR est connecté, retirez la scène VRScene de la hiérarchie
- Lancez l'application avec le bouton "Play" de Unity

### 4. Configuration Session
- Entrez un nom d'utilisateur
- Pour le champ "Session", mettez "S1" pour intégrer la même session que l'utilisateur VR

## Notes
- Assurez-vous que le réseau WiFi permet la communication entre les appareils
- Vérifiez que les ports nécessaires sont ouverts pour Unity Transport
