# Use the official Prometheus image as the base
FROM prom/prometheus:latest

# Set the working directory
WORKDIR /etc/prometheus

# Copy the Prometheus configuration file into the container
COPY prometheus.yml .

# Expose the default Prometheus port
EXPOSE 9090

# Command to run Prometheus
CMD ["--config.file=/etc/prometheus/prometheus.yml"]
