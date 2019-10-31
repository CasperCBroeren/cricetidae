<template>
    <div class="topDelta">

        <div class="navigation">
            <h2>Top bonus items voor {{store}} week {{weekNumber}} </h2>
            <ul>
                <li> <a href="#/TopDelta/Ah" v-on:click="loadTopForStore('Ah')">Ah</a> | </li>
                <li> <a href="#/TopDelta/Etos" v-on:click="loadTopForStore('Etos')">Etos</a> | </li>
                <li><a href="#/TopDelta/Gall" v-on:click="loadTopForStore('Gall')">Gall&amp;Gall</a>  </li>
            </ul>
        </div>

        <BonusOverview v-on:selected="SetInformation" v-model="items" />
        <BonusProgression v-model="selectedRow" v-on:selected="SetInformation" />
    </div>
</template>

<script lang="ts">
    import { Vue, Prop, Component } from 'vue-property-decorator';
    import BonusOverview from '@/components/bonusoverview.vue';
    import BonusProgression from '@/components/bonusprogression.vue';
    import { BonusItem } from '../bonusitem';
    import { BonusService } from '../services/BonusService';
    import '../utils/Filters';

    @Component({
        components: {
            BonusProgression,
            BonusOverview,
        },
    })
    export default class TopDelta extends Vue {

        @Prop({ default: 'default value' })
        public store!: string;

        public selectedRow: (BonusItem | null);
        public items!: BonusItem[];
        public weekNumber: (number | null);

        private bonusService: BonusService;

        public constructor() {
            super();
            this.items = [];
            this.weekNumber = null;
            this.selectedRow = null;
            this.bonusService = new BonusService();
        }

        public created() {
            this.bonusService.getAllTopDelta(this.store).then((responseItems) => {
                this.items = responseItems.data;
                this.weekNumber = this.items[0].prices[0].week;
            });
        }

        public SetInformation(item: BonusItem): void {
            this.selectedRow = item;
        }
        public loadTopForStore(storeToSet: string): void {
            this.items = [];
            this.bonusService.getAllTopDelta(storeToSet).then((responseItems) => {
                this.items = responseItems.data;
            });
        }
    }
</script>
<style scoped lang="scss">
    @import "../scss/_variables.scss";

    h1 {
        color: $mainColor;
    }

    h2 {
        margin: 0px;
    }

    a {
        color: darken($mainColor, 10%);
    }

    .navigation {
        margin-bottom: 2%;
        ul

    {
        width: 100%;
        margin: 0px auto;
        list-style-type: none;
        display: inline-block;
        padding: 0px;
        li

    {
        display: inline;
 
        text-align: center;
        margin: 0px;
    }

    }
    }
</style>
