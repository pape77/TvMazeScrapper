# Tv-maze-scrapper

A scrapper for the TvMaze API

The scrapper was built following the specifications about the endpoints stated in: http://www.tvmaze.com/api

## Instructions to use:

Run a: 
```
docker-compose up
```
on the directory where the docker-compose.yaml file is.

Open and run the API project. You can any of the two endpoints to retrieve shows.

you can also call: 

```
http://localhost:YOUR_PORT/health 
```

to check the health status of the API. Must be healthy in order to run correctly.

You can fetch shows calling the endpoint specifying a page number and size:

```
https://localhost:YOUR_PORT/api/shows?page={page_number}&pageSize={page_size}
```

You can fetch a particular show like this (some shows don't have a cast in the origin):

```
https://localhost:YOUR_PORT/api/shows/{show_id}
```

## Show index
```
http://www.tvmaze.com/api#show-index
```

A list of all shows in our database, with all primary information included. You can use this endpoint for example if you want to build a local cache of all shows contained in the TVmaze database. This endpoint is paginated, with a maximum of 250 results per page. The pagination is based on show ID, e.g. page 0 will contain shows with IDs between 0 and 250. This means a single page might contain less than 250 results, in case of deletions, but it also guarantees that deletions won't cause shuffling in the page numbering for other shows.

Because of this, you can implement a daily/weekly sync simply by starting at the page number where you last left off, and be sure you won't skip over any entries. For example, if the last show in your local cache has an ID of 1800, you would start the re-sync at page number floor(1800/250) = 7. After this, simply increment the page number by 1 until you receive a HTTP 404 response code, which indicates that you've reached the end of the list.

As opposed to the other endpoints, results from the show index are cached for up to 24 hours.

```
URL: /shows?page=:num
Example: http://api.tvmaze.com/shows
Example: http://api.tvmaze.com/shows?page=1
```

## Show cast
```
http://www.tvmaze.com/api#show-cast
```
A list of main cast for a show. Each cast item is a combination of a person and a character. Items are ordered by importance, which is determined by the total number of appearances of the given character in this show.

```
URL: /shows/:id/cast
Example: http://api.tvmaze.com/shows/1/cast
```

## Rate limiting

API calls are rate limited to allow at least 20 calls every 10 seconds per IP address. If you exceed this rate, you might receive an HTTP 429 error. We say at least, because rate limiting takes place on the backend but not on the edge cache. So if your client is only requesting common/popular endpoints like shows or episodes (as opposed to more unique endpoints like searches or embedding), you're likely to never hit the limit. For an optimal throughput, simply let your client back off for a few seconds when it receives a 429.

Under special circumstances we may temporarily have to impose a stricter rate limit. So even if your client wouldn't normally exceed our rate limit, it's useful to gracefully handle HTTP 429 responses: simply retry the request after a small pause instead of treating it as a permanent failure.

While not required, we strongly recommend setting your client's HTTP User Agent to something that'll uniquely describe it. This allows us to identify your application in case of problems, or to proactively reach out to you.
