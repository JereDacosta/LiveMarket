from drain3 import TemplateMiner
from drain3.template_miner_config import TemplateMinerConfig
import re
import pandas as pd
import json

# Configure the Drain model
config = TemplateMinerConfig()
drain_parser = TemplateMiner(config=config)

# Define the regex pattern for parsing OpenTelemetry logs
log_pattern = re.compile(
    r"(?P<Date>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}-\d{2}:\d{2})\s"
    r"(?P<Metadata>\{.*?\})\s"  # Metadata JSON block
    r"(?P<Content>\{.*\})"  # Content JSON block
)

# Initialize list to hold parsed log data
log_data = []

with open('livemarket_combined_utf8.log', 'r') as log_file:
    for line in log_file:
        line = line.strip()
        match = log_pattern.match(line)
        if match:
            log_content = match.group("Content")  # Extract the Content field

            try:
                content = json.loads(log_content)
            except json.JSONDecodeError as e:
                print(f"JSON parsing error: {e}")
                continue

            log_message = content.get("body", "Unknown")

            result = drain_parser.add_log_message(log_message)

# Open and read the log file
with open('livemarket_combined_utf8.log', 'r') as log_file:
    for line in log_file:
        line = line.strip()  # Remove leading/trailing whitespace
        match = log_pattern.match(line)

        if match:
            # Extract components
            date = match.group("Date")
            metadata_json = match.group("Metadata")
            content_json = match.group("Content")

            # Parse JSON blocks
            try:
                metadata = json.loads(metadata_json)
                content = json.loads(content_json)
            except json.JSONDecodeError as e:
                print(f"JSON parsing error: {e}")
                continue

            # Extract the log message (e.g., `body`) and process it with Drain
            log_message = content.get("body", "Unknown")

            result = drain_parser.match(log_message)

            # Get matched template and cluster ID from Drain
            if result:
                matched_template = result.get_template()
                cluster_id = result.cluster_id
            else:
                matched_template = "Unknown"
                cluster_id = "Unknown"

            # Combine extracted data into a structured format
            log_entry = {
                "Date": date,
                "Instance": metadata.get("instance", "Unknown"),
                "Job": metadata.get("job", "Unknown"),
                "Level": metadata.get("level", "Unknown"),
                "Body": log_message,
                "TraceID": content.get("traceid", "Unknown"),
                "SpanID": content.get("spanid", "Unknown"),
                "Severity": content.get("severity", "Unknown"),
                "HttpMethod": content.get("attributes", {}).get("HttpMethod", "Unknown"),
                "Uri": content.get("attributes", {}).get("Uri", "Unknown"),
                "ServiceName": content.get("resources", {}).get("service.name", "Unknown"),
                "Matched Template": matched_template,
                "Cluster ID": cluster_id,
            }

            log_data.append(log_entry)

# If no data was found, print a message
if not log_data:
    print("No log data was processed. Please check the log pattern and log file content.")

# Convert the list of log entries into a DataFrame and save to a CSV file
if log_data:
    df = pd.DataFrame(log_data)
    df.to_csv('parsed-opentelemetry-logs.csv', index=False)
    print("Log data has been parsed and saved to 'parsed-opentelemetry-logs.csv'.")
