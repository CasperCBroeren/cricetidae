import { BonusPrice } from './bonusprice';

export class BonusItem {
    constructor(
        public bonusOccurance: number,
        public brand: string,
        public category: string,
        public id: number,
        public link: string,
        public prices: BonusPrice[],
        public store: string,
        public title: string) {
    }
}
