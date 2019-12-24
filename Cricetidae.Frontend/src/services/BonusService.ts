import axios from 'axios';
import { BonusItem } from '../bonusitem';

export class BonusService {
    // http://cricetidea.data/api/
    private readonly Root: string = 'http://cricetidae-api.westeurope.azurecontainer.io/api/';

    public getAllBonusProducts() {
        return axios.get<BonusItem[]>(this.Root + 'MeasuredBonusProduct');
    }

    public getAllTopDelta(store: string) {
        return axios.get<BonusItem[]>(this.Root + 'TopDelta/' + store);
    }
}
