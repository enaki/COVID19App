# COVID19App

## Introduction

COVID19App is a program that shows data and statistics about COVID 19 pandemic.
The information automatically updates in order to maintain content fresh and reliable.
The application uses a local database so it can be used without Internet connection but using it this way many days in a row can lead to obsolete data.

### Product Functions

The major functions the Covid19App must perform for the end user are the following:
* __Interactive worldwide map which reflects the state of actual pandemic__ – each country is colored according to the number of reported active cases of infection; also the map is responsive to the user’s mouse hovering or clicking, displaying relevant information.
* __Worldwide statistics__ – the application presents concrete statistics about the evolution of the SARS-CoV-2 virus spreading and about its effects on global population.
* __Country level statistics__ - the application presents concrete statistics about the evolution and state of the epidemic in a specific country.

## User Interface
### Map View

![Map View](https://github.com/enaki/ProiectIP/blob/master/Documentation/ss/map_view.png)


This view (tab) is global map which graphically indicates a general situation about total cases confirmed.
Color scheme:

Starting from 0 to Max:
* __Gray__ - no data provided
* __Green__ - fewest cases confirmed
* __Yellow__
* __Orange__
* __Red__ - most cases confirmed

###### Scale:

For a better appearance and a more intuitive overview, map uses logarithmic scale because the numerical data covers a very wide range of values.

###### Time axis

By default, the display information corresponds to the most recent date from database (or if available, from Internet).
In the bottom of the screen you can change that by scrolling the track bar.

###### Click event

You can click on any country for more information. When you do that, the country view tab opens with data and charts about the country you selected.

### Global View

![Global View](https://github.com/enaki/ProiectIP/blob/master/Documentation/ss/global_view.png)

This view (tab) is chart which graphically indicates a general situation about total cases confirmed grouped by continent.

 ###### The Chart

The chart shows total cases confirmed, group by continent.
On the X-axis is time, starting from 22.01.2020 until the most recent day in the database (most of the times, this will be today date).
On the Y-axis is number of people affected by virus.

###### The Gauges

* The first gauge shows the percentage of people infected (number of people infected / population of all countries * 100).
* The second gauge shows the mortality rate of COVID 19 (number of deaths cause by virus / number of people infected * 100).
* The third gauge shows the healing rate of COVID 19 (number of deaths healed/ number of people infected * 100).


### Country View

![Country View](https://github.com/enaki/ProiectIP/blob/master/Documentation/ss/country_view.png)


This view (tab) is chart which graphically indicates a country situation.
This view differs according to country is displayed. You can change the country by going to map view tab and click on another country.

###### Overview:

This view (tab) is chart which graphically indicates a country situation.
This view differs according to country is displayed. You can change the country by going to map view tab and click on another country.

###### The Chart:
The chart indicates a country situation over time. You can see the trend of infections, deaths and healing.
On the X-axis is time, starting from 22.01.2020 until the most recent day in the database (most of the times, this will be today date).
On the Y-axis is number of people affected by virus. By adding death, active and recovered you obtain total cases confirmed.

###### Color Scheme:
* Red - deaths
* Yellow - active cases
* Green - recovered

###### The Gauges:
* The first gauge shows the absolute value of people infected.
* The second gauge shows the absolute of deaths.
* The third gauge shows the absolute of recovered people.


## ER Diagram

![ER Diagram](https://github.com/enaki/ProiectIP/blob/master/Documentation/database/COVID_database_relational_model.png)


## Class Diagram

![Class Diagram](https://github.com/enaki/ProiectIP/blob/master/Documentation/diagrams/images/class_diagram.png)


## Activity Diagram

![Activity Diagram](https://github.com/enaki/ProiectIP/blob/master/Documentation/diagrams/images/COVID-Activity-Diagram.png)


## Sequence Diagram

![Sequence Diagram](https://github.com/enaki/ProiectIP/blob/master/Documentation/diagrams/images/COVID-Sequence-Diagram.png)


## Use-Case Diagram

![Use-Case Diagram](https://github.com/enaki/ProiectIP/blob/master/Documentation/diagrams/images/COVID-UseCase-Diagram.png)


## Notes

1. Run the script `renew.bat` located in `scripts` in order to create the database.