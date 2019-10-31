export class BonusPrice {
    constructor(
        public activeAtNumberOfProducts: number,
        public delta: number,
        public discountText: string,
        public endDate: string,
        public forPriceInCents: number,
        public fromPriceInCents: number,
        public startDate: string,
        public unitSize: string,
        public week: number,
        public year: number) {

    }
}
