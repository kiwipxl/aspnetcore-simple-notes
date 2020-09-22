As a learning experiment, I built a simple API using ASP.NET Core and Entity Framework.

# API
### Get Notes
`GET /api/notes`

Returns the most recently created notes.
Required `start` and `count` query parameters. For example: `GET /api/notes?start=0&count=10`.

Optionally can filter by `labels` query parameter. Example: `GET /api/notes?start=0&count=10&labels=science,psychology`

### Get Note
`GET /api/notes/:id`

### Create Note
`POST /api/notes`

Example request body:
```JSON
{
	"title": "Bought a turtle today!", 
	"body": "His name is Jerry.", 
	"labels": [
		{
			"name": "Jerry"
		}
	]
}
```

### Update Label
`PUT /api/notes/:id`

Example request body:
```JSON
{
	"id": 12, 
	"title": "Bought a TURTLE today!!! :)"
}
```

### Get Labels
`GET /api/labels/`

Returns a list of ALL labels.

### Get Label
`GET /api/labels/:id`

### Create Label
`POST /api/labels`

Example request body:
```JSON
{
	"name": "Science"
}
```

### Update Label
`PUT /api/labels/:id`

Example request body:
```JSON
{
	"id": 4, 
	"name": "NewScience"
}
```