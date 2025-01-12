<script>
    import store from "../../state/store";
    export default {
        data(){
            return{
                postData:[],
                customerId: 0,
            }
        },
        created(){
            this.customerId = store.getters['getCustomerId'] ?? 0;
        },
        methods:{

        }
    }
</script>
<template>
        <v-container>
            <v-snackbar
                v-model="snackbar"
                :timeout="10000"
                class="custom-snackbar"
                :color="snackbarColor"
            >
                <h5 class="text-center">{{ message }}</h5>
            </v-snackbar>
            <v-row>
                <v-col md="2"></v-col>
                <v-col cols="12" md="8">
                    <h3>Danh sách các bài đăng của bạn</h3>
                    <BCard no-body>
                        <BCardBody class="p-0">
                            <BRow>
                                <BCol class="col-md-4 col-12">
                                    <v-window style="height: 100%;"
                                        v-model="window"
                                        show-arrows
                                        theme="dark"
                                    >
                                        <v-window-item
                                        v-for="n in length"
                                        :key="n"
                                        >
                                        <v-card class="d-flex justify-center align-center" height="200px">
                                            <v-img
                                                src="/images/Room/20241119180012-04a6_wm.jpg"
                                                alt="Room Image"
                                                aspect-ratio="16/9"
                                                class="rounded elevation-2"
                                                contain
                                            ></v-img>
                                        </v-card>
                                        </v-window-item>
                                    </v-window>
                                </BCol>
                                <BCol class="col-md-8 col-12 mt-4">
                                    <router-link to="/detail">
                                        <h4> Số 11 Ngõ 91 đường Cầu Diễn </h4>
                                        <h5>Giá: <span style="color: red;">3.000.000 VNĐ</span></h5>
                                        <p><v-icon>mdi-map-marker-radius</v-icon> Số 11 Ngõ 91 đường Cầu Diễn Phường Cầu Diễn </p>
                                        <p>Giới tính: Nam</p>
                                    </router-link>
                                    <div class="text-right">
                                        <BButton variant="white"><v-icon>mdi-heart-outline</v-icon></BButton>
                                    </div>
                                </BCol>
                            </BRow>
                        </BCardBody>
                    </BCard>
                </v-col>
            </v-row>
        </v-container>
</template>