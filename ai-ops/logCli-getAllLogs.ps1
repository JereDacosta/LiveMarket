# Define your query parameters
$startTime = "2024-11-24T15:00:00Z"
$endTime = "2024-11-26T00:00:00Z"
$query = '{exporter=\"OTLP\"}'
$outputFile = "livemarket_combined.csv"

# Define the time window for each query (e.g., 5 minutes)
$interval = 1 # minutes
$startDate = [datetime]::ParseExact($startTime, "yyyy-MM-ddTHH:mm:ssZ", $null)
$endDate = [datetime]::ParseExact($endTime, "yyyy-MM-ddTHH:mm:ssZ", $null)

# Loop through the time period in 5 minute intervals
while ($startDate -lt $endDate) {
    # Format the time range for the current query
    $currentStartTime = $startDate.ToString("yyyy-MM-ddTHH:mm:ssZ")
    $currentEndTime = $startDate.AddMinutes($interval).ToString("yyyy-MM-ddTHH:mm:ssZ")
    
    # Run the logcli query
    Write-Host "Fetching logs from $currentStartTime to $currentEndTime"
    .\logcli-windows-amd64.exe query --from=$currentStartTime --to=$currentEndTime --limit=5000 --merge-parts $query >> $outputFile

    # Wait for the next interval (5 minutes)
    $startDate = $startDate.AddMinutes($interval)
    
    # Sleep for 5 minutes (optional, for controlling the script's pacing)
    Start-Sleep -Seconds 1
}

Write-Host "All logs have been fetched and merged into $outputFile"
