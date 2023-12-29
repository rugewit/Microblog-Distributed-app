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

![1](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/1.png)

Работает

![2](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/2.png)

Отключим 1 монго-ноду

![3](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/3.png)

Работает

![4](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/4.png)

Отключим еще 1 монго-ноду

![5](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/5.png)

Ошибка

![6](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/6.png)

![7](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/7.png)

Восстановим монго-ноды

![8](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/8.png)

Работает

![9](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/9.png)

Отключим 1 elastic ноду

![10](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/10.png)

Работает

![11](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/11.png)

Отключим еще одну elastic ноду

![12](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/12.png)

Работает

![13](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/13.png)

Отключим последнюю elastic ноду

![14](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/14.png)

Перестало работать

![15](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/15.png)

Восстановим elastic ноды

![16](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/16.png)

Работает

![17](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/17.png)

Отключим 2 апи ноды

![18](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/18.png)

Работает

![19](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/19.png)

Отключим последнюю апи ноду

![20](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/20.png)

Теперь не работает

![21](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/21.png)

Восстановим апи ноды

![22](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/22.png)

Работает

![23](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/23.png)

Отключим 2 кеш ноды

![24](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/24.png)

Работает

![25](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/25.png)

Отключим последнюю кеш ноду

![26](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/26.png)

Работает. но все значения возвращаются из монго

![27](https://github.com/rugewit/Microblog-Distributed-app/blob/main/MicroBlog/Images/27.png)
