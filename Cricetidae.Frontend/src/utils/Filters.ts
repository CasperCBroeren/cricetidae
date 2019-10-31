import { Vue } from 'vue-property-decorator';

const asEuro = (value: string) => {
    const euroValue: number = (Number(value) / 100);
    return euroValue.toFixed(2);
};

const fixUnknown = (value: string) => {
    return value == null || value === '' ? 'Onbekend' : value;
};

Vue.filter('fixUnknown', fixUnknown);
Vue.filter('asEuro', asEuro);
