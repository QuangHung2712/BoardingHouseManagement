<template>
<v-main>
  <v-row class="m0 header d-flex">
    <v-col cols="4" >
      <button class="text-left text-header">QUẢN LÝ NHÀ TRỌ</button>
    </v-col>
    <v-col cols="4">
      <v-row v-show="!AddressTower" class="m0 d-flex align-center">
        <v-col cols="3" class="p0 mb-5 title-address">
          Địa chỉ: 
        </v-col>
        <v-col cols="9" class="p0">
          <v-select 
          v-model="selectTower"
          :items="AddressTower"
          item-title="title"
          item-value="value"></v-select>
        </v-col>
      </v-row>
    </v-col>
    <v-col cols="4" class="p0	">
      <button @click="diglogProfile = !diglogProfile" class=" profile d-flex mt-8 mr-5">
        <p>{{ NameLandlord }}</p>
        <v-icon icon="mdi-menu-down"></v-icon>
      </button>
      <div v-show="diglogProfile" class="diglog_Profile">
          <button class="d-flex">
            <v-icon icon="mdi-account"></v-icon>
            <span class="ml-2">Thông tin cá nhân</span>
          </button>
          <button class="d-flex mt-3">
            <v-icon icon="mdi-key"></v-icon>
            <span class="ml-2">Đổi mật khẩu</span>
          </button>
          <button class="d-flex mt-3">
            <v-icon icon="mdi-exit-to-app"></v-icon>
            <span class="ml-2">Đăng xuất</span>
          </button>
      </div>
    </v-col>
  </v-row>
  <router-link to="/"></router-link>
  <router-view></router-view>
</v-main>
</template>
<script>
  export default {
    data(){
      return{
        AddressTower:[
          {title: "Số 11 ngõ 91 Cầu Diễn", value: 0},
          {title:"Trâu Quỳ Gia Lâm", value:1}
        ],
        selectTower:0,
        NameLandlord: 'Phạm Quang Hưng',
        diglogProfile: false
      }
    },
    methods: {
      setActiveMenu(menuIndex) {
      this.activeMenu = menuIndex;
      this.Menu = menuIndex
      }
    }
  }
</script>
<style scoped>
.m0{
  margin: 0;
}
.p0{
  padding: 0;
}
.text-header{
  font-size: 200%;
  margin-left: 20px;
}
.title-address{
  font-size: 130%;
}
.header{
  background-color: #fed9c2;
  height: 12vh;
}
.profile{
  width: 100%;
  justify-content: flex-end;
  padding-right: 30px;
}
.diglog_Profile{
  width: 40%;
  border: 1px solid black;
  background-color: aqua;
  margin-left: 50%;
  margin-top: 1%;
  padding: 2%;
  border-radius: 10px;
}
</style>