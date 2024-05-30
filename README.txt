docker pull postgres

docker run --name TourPlanerContainer -e POSTGRES_PASSWORD=Debian123! -d -p 5432:5432 postgres

docker exec -it TourPlanerContainer psql -U postgres -c "CREATE DATABASE TourDB;"
