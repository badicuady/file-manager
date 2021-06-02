interface Setting {
    apiUrl: string
}

interface Configuration {
    [index:string]: Setting,
    dev: Setting;
    test: Setting;
    prod: Setting;
}

const settingsDev: Setting = {
    apiUrl: 'http://localhost:5000/graphql'
}

const settingsTest: Setting = {
    apiUrl: 'http://localhost:5000/graphql'
}

const settingsProd: Setting = {
    apiUrl: 'http://localhost:5000/graphql'
}

const settings: Configuration = {
    dev: settingsDev,
    test: settingsTest,
    prod: settingsProd
};

const environment:string = process.env.ENVIRONMENT || 'dev';
const s: Setting = settings[environment];
export default s;