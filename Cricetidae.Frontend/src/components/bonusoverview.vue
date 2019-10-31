<template>
    <div class="mainbox">

        <div v-if="items.length > 0">
            
            <table class="largeDataTable">
                <thead>
                    <tr>
                        <th><abbr title="Aantal keer in de bonus">#</abbr></th>
                        <th>Product</th>
                        <th>Van</th>
                        <th>Voor</th>
                        <th>Bonus</th>
                        <th>Aanbieding</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in items" v-on:click="viewItem(item)">
                        <td>
                            {{item.bonusOccurance}}
                        </td>
                        <td>
                            <a v-bind:href="'https://www.ah.nl'+item.link" target="_blank">{{item.title}}</a>
                        </td>
                        <td>
                            <s>&euro; {{item.prices[0].fromPriceInCents| asEuro}}</s>
                        </td>
                        <td>
                            &euro; {{item.prices[0].forPriceInCents | asEuro}}
                        </td>
                        <td>
                            &euro; {{item.prices[0].delta | asEuro}}
                        </td>

                        <td>
                            {{item.prices[0].discountText}}
                        </td>

                    </tr>
                </tbody>
            </table>
        </div>
        <div v-else>
            hamsters laden...
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Model, Vue } from 'vue-property-decorator';
    import { BonusItem } from '../bonusitem';

    import '../utils/Filters';

    @Component
    export default class BonusOverview extends Vue {
        @Model()
        public items!: BonusItem[];

        constructor() {
            super();
        }

        public viewItem(item: BonusItem) {
            this.$emit('selected', item);
        }
    }
</script>

<style scoped lang="scss">
    @import "../scss/_variables.scss";
         a {
        color: darken($mainColor, 10%);
    }

    table.largeDataTable {
        background-color: $tableMain;
        width: 100%;
        text-align: center;
        border-spacing: 0px;
        border-collapse: separate;
        td, th

    {
        padding: 4px 0px;
    }

    tbody {
        td

    {
        font-size: 12px;
        border-bottom: 0.001rem solid darken($tableMain, 50%);
        cursor: pointer;
    }

    }

    thead {
        background: #FF7900;
        th

    {
        font-size: 11px;
        font-weight: bold;
        color: #ffffff;
        text-align: center;
    }

    th:first-child {
        border-left: none;
    }

    }
    }
</style>
