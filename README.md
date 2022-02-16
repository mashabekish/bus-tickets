# Bus ticket sales

.NET Core console application for bus ticket sales

## MENU
 - [DESCRIPTION](#description)
 - [CONSTANTS](#constants)
 - [RUN WITH CODE](#run-with-code)

### <a id="description"></a>DESCRIPTION:
Sale of bus tickets for flights
 - Authorization to log in to the system
 - Admin and user functionality
 - Purchase of tickets for a specific flight in the required quantity

### <a id="constants"></a>CONSTANTS:
 - `host` - mysql host. Default: `localhost`
 - `port` - mysql port. Default: `3306`
 - `user` - mysql user. Default: `root`
 - `password` - mysql password. Default: `root1234`
 - `database` - mysql database. Default: `bus_tickets`

### <a id="run-with-code"></a>RUN WITH CODE:
  - Clone
      ```bash
      git clone https://github.com/mashabekish/bus-tickets.git
      ```
  - Run docker container `mysql` in `docker-compose.yml`
      ```bash
      docker-compose up
      ```
  - Run the project
