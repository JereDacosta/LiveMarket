# Specify the input (UTF-16) and output (UTF-8) file paths
input_file = "livemarket_combined.log"
output_file = "livemarket_combined_utf8.log"

# Open the UTF-16 file and write it as UTF-8
with open(input_file, "r", encoding="utf-16") as infile, open(output_file, "w", encoding="utf-8") as outfile:
    for line in infile:
        outfile.write(line)

print(f"File successfully converted to UTF-8: {output_file}")
