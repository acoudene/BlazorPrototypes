_2024-10-17 - Anthony Coudène - Création_

# Le mode simple utilisant une gestion des events classiques C#

![Picture1.png](./Images/Picture1.png)

via : 

- Une page : **SimpleEventHome**
- Un ViewModel : **SimpleEventViewModel**
- 2 arguments d'events classiques : **ProcessingEventArgs** et **ProcessedEventArgs**

On retrouve du code classique d'abonnement :

![Picture2.png](./Images/Picture2.png)

Et ici : l'envoi de notification d'event :

![Picture3.png](./Images/Picture3.png)

Dans le 2ème cas accessible par le NavLink ou le path relatif /eventbus, on va retrouver une autre approche que j'ai isolée très vite fait (attention code à ne pas reprendre encore en production), la suite.

# Le mode avec Event Bus local

![Picture4.png](./Images/Picture4.png)

- Une page : **EventBusHome**
- Un ViewModel injecté : **EventBusViewModel**
- 2 events autosuffisants (message+data) : **ProcessingBusEvent** et **ProcessedBusEvent**
- Un bus d'évènement local au process (attention le terme est le même que celui des MOMs mais ce n'est pas un MOM !!!!) : **IEventBus**

Voici le nouveau code d'abonnement :

![Picture5.png](./Images/Picture5.png)

Et le code de notification :

![Picture6.png](./Images/Picture6.png)

On voit qu'on ne gère plus que du C# sans se soucier de la gestion évènementielle qui peut induire des memory leaks ou des problèmes particuliers si on ne respecte pas les best practices.

Pour initialiser le tout, on va avoir la main sur les durées de vie et le partage des espaces de notifications. Ici, j'ai choisi un singleton par exemple :

![Picture7.png](./Images/Picture7.png)

On parle de bus d'évènement mais il s'agit ici d'une gestion encapsulée des events C# au sein d'un même processus.



