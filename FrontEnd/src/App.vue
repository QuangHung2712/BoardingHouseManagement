<template>
  <v-app>
    <v-main>
      <router-link to="/"></router-link>
      <router-view></router-view>
    </v-main>
  </v-app>
</template>

<script>

export default {
  name: 'App',

  data: () => ({
    //
  }),
}
</script>
