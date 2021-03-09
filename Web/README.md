## Dependencias
* `node` and `npm` (`brew install node`)

## Instalación
* `npm install -g webpack-dev-server rimraf webpack karma-cli protractor typescript cross-env`
* `npm install` to install all dependencies or `yarn`

## Comandos para el proyecto web

### Para comenzar
```bash
npm run server:gov
```
  
### Servidor
```bash
# Desarrollo
npm run server
# Producción
npm run build:prod
npm run server:prod
```

### Construccion del producto
```bash
# Desarrollo
npm run build:dev
# Producción (jit)
npm run build:prod
# AoT
npm run build:aot
```

### Hot Module Replacement
```bash
npm run server:dev:hmr
```

### Correr Tests Unitarios
```bash
npm run test
```

### Correr Tests Unitarios continuamente
```bash
npm run watch:test
```

### Correr Tests de Sistema
```bash
# update Webdriver (optional, done automatically by postinstall script)
npm run webdriver:update
# this will start a test server and launch Protractor
npm run e2e
```

### Correr todos los Tests continuamente
```bash
# this will test both your JIT and AoT builds
npm run ci
```

### Correr los Tests de Sistema con Protractor's 
```bash
npm run e2e:live
```