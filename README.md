## .NET-Mentoring-Module10.HTTPfundamentals

#Задание к модулю HTTP fundamentals

Задание:

Необходимо реализовать библиотеку и использующую её консольную программу для создания локальной копии сайта («аналог» программы wget).
Работа с программой выглядит так: пользователь указывает стартовую точку (URL) и папку куда надо сохранять, а программа проходит по 
всем доступным ссылкам и рекурсивно выкачивает сайт(ы).

Опции программы/библиотеки:
•	ограничение на глубину анализа ссылок (т.е. если вы скачали страницу, которую указал пользователь, это уровень 0, все страницы на которые введут ссылки с неё, это уровень 1 и т.д.) 
•	ограничение на переход на другие домены (без ограничений/только внутри текущего домена/не выше пути в исходном URL)
•	ограничение на «расширение» скачиваемых ресурсов (можно задавать списком, например так: gif,jpeg,jpg,pdf)
•	трассировка (verbose режим): показ на экране текущей обрабатываемой страницы/документа

Рекомендации по реализации:
В качестве основы можно взять следующие библиотеки:
  -Работа с http:
o	System.Net.Http.HttpClient – рекомендуемый вариант
o	System.Net.HttpWebRequest – legacy 
	-Работа с HTML:
o	Можно воспользоваться одной из библиотек, перечисленных тут
o	Самый популярный вариант HtmlAgilityPack, хотя он достаточно и старый и имеет свои проблемы. 

