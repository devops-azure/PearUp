FROM microsoft/dotnet:2.0.3-sdk as Builder
WORKDIR /PearUp.Api
RUN apt update && \
    apt install -y unzip
# Copy app source
COPY . .
# Install sonar-scanner and use host sonar properties
RUN curl -SL https://sonarsource.bintray.com/Distribution/sonar-scanner-cli/sonar-scanner-cli-3.0.3.778-linux.zip --output sonar-scanner.zip && \
    unzip -d /usr/share/ sonar-scanner.zip && \
    ln -s /usr/share/sonar-scanner-3.0.3.778-linux/bin/sonar-scanner /usr/bin/sonar-scanner && \
    rm -rf /usr/share/sonar-scanner-3.0.3.778-linux/conf/sonar-scanner.properties && \
    cp sonar-scanner.properties /usr/share/sonar-scanner-3.0.3.778-linux/conf/sonar-scanner.properties
# Restore dependencies and build at out
RUN dotnet restore
RUN dotnet publish --output ./out
# run sonar-scanner
RUN sonar-scanner

# Build runtime image
FROM microsoft/aspnetcore
WORKDIR /PearUp.Api
# Copy executables from previous container
COPY --from=Builder /PearUp.Api/PearUp.Api/out .
EXPOSE 61167
ENV ASPNETCORE_URLS http://0.0.0.0:61167
# Run dotnet main executable
CMD ["dotnet", "PearUp.Api.dll"]

