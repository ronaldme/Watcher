With this application you can keep track of your favorite shows, movies and persons.

Global application structure:
- Backend service / console application using TopShelf
- Web application in ASP.NET MVC

The backend service notifies users when new movies, shows or their favorite person releases a new production.
Using the web interface users can subscribe/unsubscribe and view their settings.

The data is retrieved from TheMovieDb. For this to work you will need to add your TheMovieDb key to the appSettings.

The data for new releases is refreshed on a specified interval. This is set to 12 hours (43200000 ms) by default.

A user can be notified through email or NotifyMyAndroid.