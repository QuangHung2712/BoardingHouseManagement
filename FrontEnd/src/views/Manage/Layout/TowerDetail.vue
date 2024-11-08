<template>
      <v-row class="m0">
    <v-col cols="2" class="menu">
      <div >
        <router-link 
          @click="setActiveMenu(0)"
          :to="`/manage/${towerId}/home`" 
          class="menuitem"
          :class="{'active-menu': activeMenu === 0}"
        >
          <v-icon icon="mdi-home-silo-outline"></v-icon>
          <span>Trang chủ</span>
        </router-link>
      </div>
      <div >
        <router-link 
          @click="setActiveMenu(1)"
          :to="{name:'room', params: {idtower: towerId}}" 
          class="menuitem"
          :class="{'active-menu': activeMenu === 1}"
        >
          <v-icon>mdi-home</v-icon>
          <span>Phòng</span>
        </router-link>
      </div>
      <div>
        <router-link 
          :to="`/manage/${towerId}/service`" 
          @click="setActiveMenu(2)" 
          :class="{'active-menu': activeMenu === 2}" 
          class="menuitem">
          <v-icon></v-icon>
          <span>Dịch vụ</span>
        </router-link>
      </div>
      <div>
        <router-link  
          @click="setActiveMenu(3)" 
          :class="{'active-menu': activeMenu === 3}" 
          class="menuitem">
          <v-icon icon="mdi-flash"></v-icon>
          <span>Số điện</span>
        </router-link >
      </div>
      <div>
        <router-link 
          @click="setActiveMenu(4)" 
          :to="`/manage/${towerId}/bill`"
          :class="{'active-menu': activeMenu === 4}" 
          class="menuitem">
          <v-icon icon="mdi-water"></v-icon>
          <span>Hóa đơn</span>
        </router-link>
      </div>
      <div>
        <router-link 
          :to="`/manage/${towerId}/contract`"
          @click="setActiveMenu(5)" 
          :class="{'active-menu': activeMenu === 5}" 
          class="menuitem">
          <span>Hợp đồng</span>
        </router-link>
      </div>
      <div>
        <router-link 
          :to="`/manage/${towerId}/arise`"
          @click="setActiveMenu(6)" 
          :class="{'active-menu': activeMenu === 6}" 
          class="menuitem">
          <v-icon icon="mdi-cash"></v-icon>
          <span>Phát sinh</span>
        </router-link>
      </div>
      <div>
        <router-link 
          @click="setActiveMenu(7)" 
          :class="{'active-menu': activeMenu === 7}" 
          class="menuitem">
          <v-icon icon="mdi-chart-line"></v-icon>
          <span>Báo cáo</span>
        </router-link>
      </div>
      <div>
        <router-link 
          to="/manage/"
          class="menuitem">
          <v-icon icon="mdi-home-city"></v-icon>
          <span>Khu nhà</span>
        </router-link>
      </div>
    </v-col>
    <v-col cols="10">
      <router-view ></router-view>
    </v-col>
  </v-row>
</template>
<script>
    //import CryptoJS from 'crypto-js';
    export default{
        data(){
            return{
                activeMenu: null,
                Menu:0,
                towerId : 0,
            }
        },
        mounted() {
          const idtower = this.$route.params.idtower;
          //Giải mã
          //const DecodingIdTower = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idtower));
          this.towerId = idtower;
        },
        methods:{
            setActiveMenu(menuIndex) {
                this.activeMenu = menuIndex;
                this.Menu = menuIndex
            },
        }
    }
</script>
<style scoped>
.menu{
  background-color: #fed9c2;
  height: 88vh;
  display: flex;
  flex-direction: column;
  position: sticky;
  top: 12%;
  z-index: 1000;

}
.menu div{
  flex: 1;
}
.menu .menuitem{
  padding-left:30px;
  display: flex;
  align-items: center;
  font-size: 140%;
  color: black;
  text-decoration: none !important;
}
.menu .menuitem span{
  margin-left: 5px;
  margin-top: 5px;
}
.active-menu {
  background-color: #fcb93c;
  color: white !important;
  border-radius: 20px;
}
.menu button{
  width: 100%;
  text-align: left;
}
</style>