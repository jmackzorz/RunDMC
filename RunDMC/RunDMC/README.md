1# \# Run DMC (Data Metrics Collector)

# 

# \## Overview

# Run DMC is a fitness tracking API that records workouts and provides analytical insights. The goal is to demonstrate time-series data handling, aggregation queries, and basic analytics.

# 

# \## Features

# \- Log workouts (distance, duration, calories)

# \- Track running, cycling, and strength sessions

# \- Aggregate stats (weekly/monthly summaries)

# \- Personal records tracking

# \- Trend analysis (improving or declining performance)

# 

# \## Tech Stack

# \- ASP.NET Core Web API

# \- Entity Framework Core

# \- SQL Server

# \- LINQ for aggregation queries

# \- AutoMapper

# 

# \## Architecture

# \- Domain-driven design (lightweight)

# \- Entities:

# &#x20; - User

# &#x20; - Workout

# &#x20; - ActivityType

# \- Services handle analytics calculations

# 

# \## Key Concepts

# \- Time-series data handling

# \- Aggregation queries (GROUP BY)

# \- DTO mapping

# 

# \## Example Endpoints

# \- POST /api/workouts

# \- GET /api/workouts/user/{id}

# \- GET /api/stats/weekly

# \- GET /api/stats/personal-records

# 

# \## Sample Analytics

# \- Total distance per week

# \- Average pace

# \- Fastest run

# \- Longest session

# 

# \## Stretch Goals

# \- Add chart-ready endpoints

# \- Integrate SignalR for live tracking

# \- Export data as CSV

