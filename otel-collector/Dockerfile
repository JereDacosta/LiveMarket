# Use OpenTelemetry Collector base image
FROM otel/opentelemetry-collector-contrib:latest

# Copy the OpenTelemetry Collector config file into the container
COPY otel-collector-config.yaml /etc/otel-collector-config.yaml

# Run the collector with the provided config file
CMD ["--config=/etc/otel-collector-config.yaml"]