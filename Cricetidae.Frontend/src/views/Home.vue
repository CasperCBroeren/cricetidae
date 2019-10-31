<template>
    <div class="home">
        <h2>Week {{weekNumber}}</h2>
        <section class="filterControls">
            <label for="storeSelector">
                Winkel
            </label>
            <select id="storeSelector" v-model="selectedStore">
                <option v-for="store in stores">{{store}}</option>
            </select>
            <label for="storeSelector">
                Categorie
            </label>
            <select id="categorySelector" v-model="selectedCategory">
                <option v-for="category in categories[selectedStore]" v-bind:value="category">{{category | fixUnknown}}</option>
            </select>
        </section>
        <BonusOverview v-on:selected="SetInformation" v-model="filteredItems" />
        <BonusProgression v-model="selectedRow" v-on:selected="SetInformation" />

    </div>
</template>

<script lang="ts">
    import { Component, Watch, Vue } from 'vue-property-decorator';
    import BonusOverview from '@/components/bonusoverview.vue';
    import BonusProgression from '@/components/bonusprogression.vue';
    import { BonusItem } from '../bonusitem';
    import { BonusService } from '../services/BonusService';


    @Component({
        components: {
            BonusOverview,
            BonusProgression,
        },
    })
    export default class Home extends Vue {

        public selectedRow: (BonusItem | null);

        public filteredItems: BonusItem[];
        public items: BonusItem[];
        public weekNumber: (number | null);
        public stores: string[];
        public categories: any;
        public selectedStore: string;
        public selectedCategory: string;

        private bonusService: BonusService;

        public constructor() {
            super();
            this.selectedRow = null;
            this.weekNumber = null;
            this.stores = [];
            this.categories = [];
            this.filteredItems = [];
            this.items = [];

            this.selectedStore = 'ah';
            this.selectedCategory = '';
            this.bonusService = new BonusService();
        }

        public created() {
            this.selectedRow = null;
            this.bonusService.getAllBonusProducts().then((responseItems) => {
                this.items = responseItems.data;
                this.weekNumber = this.items[0].prices[0].week;
                this.stores = Array.from(new Set(this.items.map((x) => x.store)));
                this.categories = this.items.reduce((p: any, c: BonusItem) => {
                    p[c.store] = p[c.store] || new Set();
                    p[c.store].add(c.category);
                    return p;
                }, {});
                this.SetFirstCategory();
                this.filteredItems = this.FilteredItems(this.items);
            });
        }


        public SetInformation(item: BonusItem): void {
            this.selectedRow = item;
        }

        public FilteredItems(items: BonusItem[]): BonusItem[] {
            if (items) {
                return items.filter((e) => {
                    return e.store === this.selectedStore
                        && e.category === this.selectedCategory;
                });
            }
            return [];
        }

        @Watch('selectedCategory')
        public OnChangeCategory(newItem: string, oldItem: string) {
             this.filteredItems = this.FilteredItems(this.items);
        }

        @Watch('selectedStore')
        public OnChangeStore(newItem: string, oldItem: string) {
            this.SetFirstCategory();
        }

        private SetFirstCategory() {
            const it = this.categories[this.selectedStore].values();
            this.selectedCategory = it.next().value;
        }

    }
</script>
<style scoped lang="scss">
    @import "../scss/_variables.scss";

    h1 {
        color: $mainColor;
    }

    a {
        color: darken($mainColor, 10%);
    }

    .mainbox {
        margin: 0px auto;
    }

    .filterControls {
        margin-bottom: 2%;
        label

    {
        width: 8%;
    }

    select {
        width: 35%;
    }
    }
</style>