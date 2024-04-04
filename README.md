.Net Clean Architecture
---------------

Bookify .net clean Architecture project to learn about Clean Architecture.

KeyCloak is used for Authentication and Authorization.

Under Scratches foler, there are .http files to call rest API each for docker and local run.

Use port 5435 to connect to docker postgres from local computer in Rider or VS or PgAdmin Database tool


Docker Commands
------------
- docker-compose up
- docker-compose down 
- docker-compose up --build
- docker rmi -f $(docker images -q) -> (Removes all images)
- docker-compose down -v -> (removes volume)


keycloak Admin Dashboard
----------
http://127.0.0.1:18080/ in edge coz if http is in restricted mode
http://localhost:18080/ should work in chrome
admin dashboard: UserName & Password: admin/admin

One logged in
Make sure u select Bookify Realm on the left side drop down list (Switch to Bookify from Keycloak)


TestUser Local
------------------
UserName: test2@test.com
Password: 12345

TestUser Docker
-----------------
UserName: test1@test.com
Password: 12345

Seq
----
Do it in chrome.
http://localhost:8081/#/events?range=1d


If you Get the error  => CANCELED [bookify.api internal] load build context   

Adding .idea file to the dockerignore should resolve the issue. It seems like rider locks some file during the build


For Integration Test
---------------------
app.SeedData needs to be uncommented in promgram.cs file 
Keycloak needs to be run in docker for the local app run as well.

-----------------
If you get authentication error, check if the userid in keycloak and daabase identityid is the same.
