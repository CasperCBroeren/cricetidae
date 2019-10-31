<template>
  <div id="app">
      <div id="nav">
          <router-link to="/">Overzicht</router-link> |
          <router-link to="/TopDelta/Ah">Top Bonus</router-link> |
          <router-link to="/about">Over</router-link>
      </div>
    <router-view/>
  </div>
</template>

<style lang="scss">
     @import "scss/_variables.scss";

    #app {
      font-family: 'Avenir', Helvetica, Arial, sans-serif;
      -webkit-font-smoothing: antialiased;
      -moz-osx-font-smoothing: grayscale;
      text-align: center;
      color: #2c3e50;
    }
#nav {
  padding: 30px;
  a {
   
    color: $mainColor;
    &.router-link-exact-active {
       font-weight: bold;
    }
  }
}
</style>
