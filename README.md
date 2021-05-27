# file-manager
GraphQL file manager

# Install
* Install Docker: https://www.docker.com/products/docker-desktop
* Run powershell ___.\start.sh___
* Launch browser [here](http://localhost:5000/ui/playground)

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

