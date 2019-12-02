Excel/VBA - Jouer au Scramble Duplicate---------------------------------------
Url     : http://codes-sources.commentcamarche.net/source/100473-excel-vba-jouer-au-scramble-duplicateAuteur  : carlvbDate    : 04/04/2014
Licence :
=========

Ce document intitul� � Excel/VBA - Jouer au Scramble Duplicate � issu de CommentCaMarche
(codes-sources.commentcamarche.net) est mis � disposition sous les termes de
la licence Creative Commons. Vous pouvez copier, modifier des copies de cette
source, dans les conditions fix�es par la licence, tant que cette note
appara�t clairement.

Description :
=============

Bonjour � tous,    
<br />
<br />Voici le d�but de mon projet de scramble ;)  
  
<br />La m�thode de recherche des coups est bas�e sur celle d�velopp�e par A
ppel et Jacobson dans les ann�es 80.    
<br />Le dictionnaire (fourni dans le 
zip) est pr�sent� sous forme de DAWG.    
<br />Pour l'instant, il ne trouve pa
s encore tous les top coups mais je travaille la-dessus.    
<br />A terme, je 
voudrai faire un menu o� on affronte l'ordi mais pour le moment, je souhaiterais
 recenser tous les bugs et les anomalies dans la recherche du meilleur coup.    

<br />
<br />Pour l'essayer:   
<br />- Charger le dictionnaire (fourni dans
 le zip)   
<br />- Lancer une nouvelle partie   
<br />- G�n�rer un tirage (m
anuel ou par les boutons d�di�s)   
<br />- Recherche les coups   
<br />- S�l
ectionner un coup dans la liste des coups trouv�s   
<br />- Jouer le coup s�le
ctionn�   
<br />- et ainsi de suite...   
<br />
<br />Le contenu du sac est
 affich� dans une listbox tout comme l'historique de la partie.   
<br />
<br 
/>Le temps de recherche est limit�e par d�faut � 10 secondes mais la constante c
orrespondante peut �tre modifi�e dans le code.   
<br />
<br />Le code est abo
ndament comment� mais je reste � votre disposition pour toute question.    
<br
 />
<br />Vos commentaires, suggestions et critiques sont les bienvenus ainsi q
ue les bugs du programme.    
<br />
<br />Merci d'avance. 
<br />
<br />Mis
e � jour : 
<br />- Correction de bugs et d'erreurs dans l'algortihme de recher
che des solutions (il est maintenant cens� trouver tous les tops coups, la seule
 limite devrait �tre le temps allou� aux recherches limit�es � 60 secondes par d
�faut mais modifiable dans la d�claration des constantes) 
<br />- Limitation d
es solutions � garder pour l'affichage (Par d�faut le top 30 mais �galement modi
fiable - Attention sur des cas de double joker dans le rack, on peut atteindre p
lus de 100 000 coups valides!!! ) 
<br />- Rajout d'une fonction de chargement 
de grille manuelle (pour continuer une pr�c�dente partie ou pour tester le progr
amme face � des applications similaires 
<br />- Rajout d'une v�rification de l
a pr�sence d'un mot dans le dictionnaire. 
<br />- Mise en exergue sur la grill
e des nouvelles tuiles plac�es.
<br />- plus quelques modifications mineures
