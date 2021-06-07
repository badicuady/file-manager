# file-manager
GraphQL file manager

# Install
* Install Docker: https://www.docker.com/products/docker-desktop
* Run powershell ___.\start.sh___
* Launch browser for [backend](http://localhost:5000/ui/playground)
* Launch browser for [ui](http://localhost:3000)

# To Do
### Technical Features
* Add better logging (maybe cloud logging see: https://www.datadoghq.com/)
* Add resilience capabilities (see Microsoft.Extensions.Http.Polly)
* Add more detailed audit capabilities
* Create CI/CD pipelines

### Business Features
* Add authentication with roles
* Add folder locking mechanism to notify user when he/she are using the same resource
* Add download file/directory functionality
* Add upload folder
* Add upload files in chunks to accommodate large files
* Add icons to files and folders
* Add support for multi instance backend service

# Queries

### List items in base directory
```
query Items ($path: String) {
  items(path: $path) {
    name,
    icon,
    size,
    metadata {
      name,
      creationTime,
      lastAccessTime,
      extension,
      fullName,
      attributes
    }
  }
}
```

#### Variables
```
{
  "path": "/"
}
```

### Create directory in base directory
```
mutation CreateDirectory($activeDirectory: String, $newDirectoryName: String!) {
  createDirectory(activeDirectory:$activeDirectory, createdDirectoryName: $newDirectoryName) {
    name,
    icon
  }
}
```

#### Variables
```
{
  "newDirectoryName": "test"
}
```