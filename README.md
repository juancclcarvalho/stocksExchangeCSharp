# Stocks Monitor Application

## Overview

The Stocks Monitor Application is a console-based tool designed to monitor stock prices and suggest actions based on predefined buy and sell prices.

## Features

- Monitors stock prices at regular intervals.
- Sends email notifications based on stock price conditions.

## Stocks API

- The API chosen to source the data from is <a href="https://brapi.dev/">Brapi</a>

## Used Software

- .NET 8
- C# 12.0
- Visual Studio Community 2022

## Setup

### Environment Variables

The application requires the following environment variables to be set:

- `STOCK_API_TOKEN`: Your API token for accessing stock price data.
- `SMTP_CONFIG_PATH`: Path to the SMTP server configuration JSON file.

### SMTP Configuration

Create a JSON file for SMTP server configuration with the following structure:
```json
{
  "Server": "smtp.example.com",
  "Port": 587,
  "SenderEmail": "your-email@example.com", # Used both as sender and login for the smtp server
  "Password": "your-email-password",
  "ReceiverEmail": "receiver-email@example.com" }
```
### Usage

1. Set the required environment variables.
2. Run the application with the following command-line arguments:
   - `stockSymbol`: The stock symbol to monitor.
   - `sellPrice`: The price at which to sell the stock.
   - `buyPrice`: The price at which to buy the stock.

Example:
```
./application.exe PETR4 22.67 22.59 
```


