# P.Ro.Bot
Repositori P.Ro.Bot treball per l'assignatura de Robòtica, Llenguatge i Planificació, Curs 2020-21.

## Descripció del projecte.
<table border="0"><tr><td>
<p>Vols jugar amb gossos però no et deixen tenir gossos a casa? Aquesta és la teva solució, P.Ro.Bot jugarà amb tu tot el que vulguis. 

<p>Aquest robot és un projecte de robòtica emocional que simularà certes accions que realitzen els gossos. Principalment estarà dotat de tres funcions: utilitzar la càmera per fer un seguiment d’una pilota i així poder buscar-la, realitzar un mètode de reconeixement de persones mitjançant la càmera, i per últim, a partir d’un altaveu podrà mostrar el seu estat emociona amb lladrucs, i fins i tot mourà la cua quan vulgui jugar amb tu.
  </td><td><img src="/IMG/P.Ro.Bot.png" alt="Imatge del P.Ro.Bot" title="Imatge del P.Ro.Bot." width="1024"></td></tr></table>

## Pre-requisits
<!--<a href="https://docs.unity3d.com/Packages/com.unity.scripting.python@4.0/manual/index.html">Python for Unity v.4.0 o superior</a>-->
<b>Instal·lació de les llibreries Python</b>

* collections
* face_recognition
* argparse
* imutils
* pickle
* socket
* time
* cv2
* os

<b> Creació banc d'imatges del model face recognition </b>

Executar el fitxer 'model/buildFaceDataset.py', un cop en marxa, premer la tecla 'k' per fer una captura amb la càmera.
Es necessari afegir el paràmetre --output a la crida del script python, aquest directori és on s'emmagatzema el dataset.
```
python buildFaceDataset.py --output dataset/Jeff_Goldblum
```

<b> Entrenar el model face recognition </b>

Executar el fitxer 'model/encodeFaces.py', depenent el volum del dataset, l'execusió pot demorar-se una estona.
Es necessari especificar els atributs --dataset, directori principal del dataset i --encoding, fitxer de sortida del model entrenat.
```
python encodeFaces.py --dataset dataset/Jeff_Goldblum --encodings encodings.pickle
```

<b> Modificar els paràmetres de direcció IP i port (si fos el cas) dels fitxers 'python/allRecognition.py' i 'unity/Assets/MovimentEix.cs'.  </b>

## Execució
Executar la simulació Unity i el fitxer 'python/allRecognition.py'


## Esquma de Hardware
Aquest és l'esquema de Hardware del P.Ro.Bot amb els 100€ de pressupost que teníem.

![Esquema Hardware][]

[Esquema Hardware]: /IMG/EsqHW.png "Esquema Hardware"

## Esquema de Software
Aquest és l'esquema de Software del P.Ro.Bot.

Podem trobar 2 grans mòduls independents i vinculats en Unity 3D.

* El mòdul de Visió per Computador (Face Recognition), on hem desenvolupat reconeixement d'una pilota vermella i reconeixement facial. Mitjançant Socket s'envia la informació en paquets UDP cap al mòdul Socket d'Unity.
* Mòdul Socket d'Unity rep el paquet UDP de l'execusió Python i desa el paràmetre d'acció tractat.
* El mòdul de Simulació emocional s'encarrega de dotar al P.Ro.Bot d'emocions i moviment. Aconsegueix el paràmetre acció del mòdul Socket.

![Esquema Software][]

[Esquema Software]: /IMG/EsqSW.png "Esquema Software"

## Simulacions realitzades
* Reconeixement de persones/pilota: el mòdul de Python que es dedica a fer el reconeixement ens permet executar diferents procediments en cas que estigui visualtzant l'objecte o no.
    
* Funció de Tracking de la Pilota: troba la pilota, i la retorna al Dummy que hem situat al projecte (simula el seu propietari).
 
* Respostes anímiques (tant visuals com sonores) davant certs estímuls per definir les seves emocions.
   
[![Video simulació](https://youtu.be/lDyxOJj_CUg/0.jpg)](https://youtu.be/lDyxOJj_CUg)

## Autors
 Adrià Amorós Berlanga<br>
 Alex Castro Gastón<br>
 Dani Gómez Piquer - 2060700<br>
