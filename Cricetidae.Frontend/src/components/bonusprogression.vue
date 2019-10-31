<template>
    <div class="popupBg" v-if="item"> 
        <div>
            <header>
                <h3>{{item.title}}</h3> 
                <a  v-on:click="Close">X</a>
            </header>
            <article> 
                <ul>
                    <li>
                        <label> Merk:</label> {{item.brand}}
                    </li>
                    <li>
                        <label>Categorie:</label> {{item.category}}
                    </li>

                    <li>
                        <label>Id:</label> {{item.id}}
                    </li>
                </ul>
                <table>
                    <thead>
                        <tr>
                            <td>Jaar</td>
                            <td>Week</td>
                            <td>Van</td>
                            <td>Voor</td>
                            <td>Delta</td>
                            <td>Bonus</td>
                            <td>Aantal actief</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="price in item.prices">
                            <td>
                                {{price.year}}
                            </td>
                            <td>
                                {{price.week}}
                            </td>
                            <td>
                                {{price.fromPriceInCents| asEuro}}
                            </td>
                            <td>
                                {{price.forPriceInCents | asEuro}}
                            </td>
                            <td>
                                {{price.delta | asEuro}}
                            </td>
                            <td>
                                {{price.discountText}}
                            </td>
                            <td>
                                {{price.activeAtNumberOfProducts}}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </article>
        </div>
    </div>
</template> 
<script lang="ts">
    import { Component, Model, Vue } from 'vue-property-decorator';
    import { BonusItem } from '../bonusitem';
    import '../utils/Filters';

    @Component
    export default class BonusProgression extends Vue {
        @Model()
        public item: (BonusItem | null);

        constructor() {
            super();
            this.item = null;
        }

        public Close() {
            this.$emit('selected', null);
        }
    }
</script>

<style scoped lang="scss">
    @import "../scss/_variables.scss";

    .popupBg {
        top: 0px;
        left: 0px;
        position: fixed;
        width: 100%;
        height: 100%;
        background-color: rgba(156, 156, 156, 0.48);
        
        div 
        {
            background-color: white;
            box-shadow: 2px 2px 5px 0px rgba(0,0,0,0.5);
            width: 80%;
            height: 90%;
            border-radius: 1%;
            margin: 0px auto;

            header {
                    h3
                    {
                        padding: 1% 0%;
                    }
                    a {
                        position: absolute;
                        right: 11%;
                        top: 6%;
                        text-decoration: none;
                        font-weight: bold;
                        cursor: pointer;
                    }
           }
            
            article
            {
                width: 90%;
                margin: 0px auto;

                ul
                {
                    padding-left: 0px;
                    list-style-type: none;
                    text-align: left;
                    label 
                    {
                          width: 10%;
                          display: inline-block;
                          color: darken($mainColor, 20%);
                    }
                    @media only screen and (max-width: 600px) {
                        label {
                            width: 35%;
                        }
                    }
                }

                table
                {
                    width: 100%;
                    border-spacing: 0px;
                    border-collapse: separate;
                    thead {
                                tr
                                {
                                   
                                    color: #FFF;
                                    background-color: darken($mainColor, 20%);
                                }
                            }
                    tbody
                    {
                        tr:nth-child(even) {background: #FFF}
                        tr:nth-child(odd) {background: lighten($mainColor, 50%)}
                    }
                }
            }
            
        }
    }
   
</style>
