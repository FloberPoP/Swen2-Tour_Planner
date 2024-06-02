# SWEN2 - TourPlanner
* Tobias Kastl, if22b007   
* Florian Poppinger, if22b009

## Database-Setup

```
docker pull postgres
```

```
docker run --name TourPlanerContainer -e POSTGRES_PASSWORD=Debian123! -e TZ=Europe/Berlin -d -p 5432:5432 postgres 
```

```
docker exec -it TourPlanerContainer psql -U postgres -c "CREATE DATABASE TourDB;"
```

## Git-Structure

### Tour Planner

* Actual Project with .sln, files and folders

### Documentation

* Protocol (Intermediate and new), Images, Project-Specification and Checklists