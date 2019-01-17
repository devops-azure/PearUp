FROM node:8.11.0
RUN chown -R node:node /usr/local/lib/node_modules && chown -R node:node /usr/local/bin
USER node
# Install angular cli
RUN npm install -g @angular/cli
USER root
WORKDIR /PearUp.Web
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
# Build app inside container
RUN npm install
RUN ng build --prod
#RUN ng test
# run sonar-scanner
RUN sonar-scanner
EXPOSE 4201
# Host app at 4201 when container start
CMD ng serve --host 0.0.0.0 --port 4201

