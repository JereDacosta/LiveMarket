import pandas as pd

# Load the CSV file
df = pd.read_csv('parsed-opentelemetry-logs.csv')

# Extract the first 19 characters of the timestamp (YYYY-MM-DDTHH:MM:SS)
df['Date'] = df['Date'].str[:19]

# Convert the Date column to datetime format for proper sorting and grouping
df['Date'] = pd.to_datetime(df['Date'])

# Sort the DataFrame by the Date column
df = df.sort_values(by='Date')

def split_and_aggregate_by_cluster(df, time_interval, error_threshold, anomaly_clusters):
    # Ensure the Date column is treated as datetime for grouping
    df.set_index('Date', inplace=True)

    # Aggregate cluster counts based on the given time interval
    cluster_counts = df.groupby([pd.Grouper(freq=time_interval), 'Cluster ID']).size().unstack(fill_value=0)
    cluster_counts['Anomaly'] = '0'

    for index, row in cluster_counts.iterrows():
        if anomaly_clusters:
            if row[anomaly_clusters].sum() >= error_threshold:
                cluster_counts.at[index, 'Anomaly'] = '1'
        else:
            if row.sum() >= error_threshold:
                cluster_counts.at[index, 'Anomaly'] = '1'

    if anomaly_clusters:
        cluster_counts.drop(columns=anomaly_clusters, inplace=True)

    return cluster_counts

# Parameters for the aggregation
time_interval = '1T'  # Fixed time interval of 1 minutes
anomaly_clusters = [12, 13, 14]  # IDs of anomalous clusters
error_threshold = 1  # Maximum error count before labeling as an anomaly

# Perform the split and aggregation
result_df = split_and_aggregate_by_cluster(df, time_interval, error_threshold, anomaly_clusters)

# Reset the index for better readability
result_df.reset_index(inplace=True)

# Save the result to a CSV file
result_df.to_csv('livemarket-matrix.csv', index=False)

print("Processing complete. Results saved to 'livemarket-matrix.csv'.")
