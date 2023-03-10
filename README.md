# Build for Linux on Windows

* Execute [publishLinux.bat](publishLinux.bat)
> **_NOTE:_** You must set the 4th parameter sent to [publish.bat](publish.bat) to be the path on your local machine where circulo was cloned from git. By default files go to "C:\build\linux\circulo".

# Install on Linux 

### Install Required Packages

* Download Package Info

```
sudo apt update
```

* Install openssh

```
sudo apt install openssh-server
```

* Install nginx

```
sudo apt install nginx
```

* Install Node.js

```
sudo apt install nodejs
```

* Install npm

```
sudo apt install npm
```

* Install .NET Core 7 SDK

```
sudo snap install dotnet-sdk --classic --channel=7.0
```

```
sudo snap alias dotnet-sdk.dotnet dotnet
```

* Install PostgreSQL

```
sudo apt install postgresql postgresql-contrib
```

### Copy Build from Windows to Linux

* Copy files from Windows "C:\build\linux\circulo\home\cadmin" to "/home/cadmin" on Linux

### Set Permissions

```
sudo chmod +x /home/cadmin/circulo/web/api/API.dll
```

### Install Web Node Modules

```
cd /home/cadmin/circulo/web/client && npm install
```

### Build Web

```
cd /home/cadmin/circulo/web/client && npm run build
```

### Create Database

```
sudo -u postgres psql -c "ALTER USER postgres PASSWORD 'postgres';"
```

```
sudo -u postgres psql -c "CREATE DATABASE \"Circulo\";"
```

```
sudo chmod +x /home/cadmin/circulo/scripts/init.sql
```

```
sudo PGPASSWORD="postgres" psql -d "Circulo" -U postgres -h 127.0.0.1 -a -w -f /home/cadmin/circulo/scripts/init.sql
```

### Copy nginx.conf to nginx Directory

```
sudo cp /home/cadmin/circulo/nginx.conf /etc/nginx/nginx.conf
```

### Restart nginx

```
sudo systemctl restart nginx
```

### Trust Dev Certificate

```
dotnet dev-certs https --trust
```

### Copy API.service to system Directory

```
sudo cp /home/cadmin/circulo/web/api/API.service /etc/systemd/system/API.service
```

### Reload Daemon

```
sudo systemctl daemon-reload
```

### Enable and Start API

```
sudo systemctl enable API
sudo systemctl start API
```

# Run

* Launch https://localhost in browser on Linux machine
* Proceed to trust certificate

# Generate Certificate and Key

```
openssl req -x509 -nodes -sha256 -days 3650 -newkey rsa:4096 -keyout server-key.pem -out server-cert.pem -subj "/C=US/ST=Ohio/L=Columbus/O=Circulo LLC/OU=IT/CN=*.circulohealth.com/emailAddress=support@circulohealth.com"
```
