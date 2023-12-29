# Microblog-Distributed-app

**ФИО**: Чернобаев Андрей Александрович, М8О-114М-23<br>

**Предмет**: Разработка распределённых приложений<br>
**Цель проекта**: Распределенное приложение на MongoDB, ElasticSearch и Hazelcast на предложенном шаблона Spring Boot. Язык программирования - Java. Можно делать на аналогичном стеке на C# или другой платформе, при условии использования MongoDB, ElasticSearch и сохранении всех функциональных требований.<br>

В рамках проекта, нужно разработать web-приложение на Spring Boot с необходимым функционалом, которое подключается к MongoDB, ES и поднимает внутри себя узел Hazelcast. Приложение будет разворачиваться в кластере из 4 узлов и работать через Load Balancer, согласно референсной архитектуре.<br>

**Стек проекта:** C#, ASP.NET CORE, .NET 7, Memcached, Redis, Mongo db, Xunit, ElasticSearch<br>

**Backend API:**  C#, ASP.NET CORE, .NET 7<br>

**Кеш для юзераккаунтов**: Memcached<br>

**Кеш для локов**: Redis<br>

**БД**: Mongo db<br>

**Поисковой движок**: ElasticSearch<br>

**Тесты**: Xunit<br>

### Проверка работоспособности при отключении узлов

Запускаем приложение

![1](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/1.png)

Работает

![2](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/2.png)

Отключим 1 монго-ноду

![3](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/3.png)

Работает

![4](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/4.png)

Отключим еще 1 монго-ноду

![5](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/5.png)

Ошибка

![6](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/6.png)

![7](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/7.png)

Восстановим монго-ноды

![8](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/8.png)

Работает

![9](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/9.png)

Отключим 1 elastic ноду

![10](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/10.png)

Работает

![11](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/11.png)

Отключим еще одну elastic ноду

![12](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/12.png)

Работает

![13](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/13.png)

Отключим последнюю elastic ноду

![14](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/14.png)

Перестало работать

![15](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/15.png)

Восстановим elastic ноды

![16](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/16.png)

Работает

![17](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/17.png)

Отключим 2 апи ноды

![18](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/18.png)

Работает

![19](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/19.png)

Отключим последнюю апи ноду

![20](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/20.png)

Теперь не работает

![21](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/21.png)

Восстановим апи ноды

![22](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/22.png)

Работает

![23](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/23.png)

Отключим 2 кеш ноды

![24](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/24.png)

Работает

![25](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/25.png)

Отключим последнюю кеш ноду

![26](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/26.png)

Работает. но все значения возвращаются из монго

![27](/home/rugewit/Programming/Microblog-Distributed-app/MicroBlog/Images/27.png)
