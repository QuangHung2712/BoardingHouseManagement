<script>
    import apiClient from "@/plugins/axios";
    import store from "../../state/store";
    import CryptoJS from 'crypto-js';

    export default {
        data(){
            return{
                postData:[],
                customerId: 0,
                message: '',
                snackbar: false,
                snackbarColor: '',
            }
        },
        created(){
            this.customerId = store.getters['getCustomerId'] ?? 0;
            this.GetAllPost();
            
        },
        methods:{
            GetAllPost(){
                apiClient.get(`/Post/GetByCustomer?customerId=${this.customerId}`)
                    .then(response=>{
                        this.postData = response.data;
                        console.log(this.postData);
                    })
                    .catch(error=>{
                        this.message = "Lấy danh sách phòng lưu bị lỗi " + error.response?.data?.message || error.message;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
            },
            Edit(postId){
                const encryptedId = CryptoJS.enc.Base64.stringify(CryptoJS.enc.Utf8.parse(postId));
                this.$router.push({ name: 'editpost', params: { idpost: encryptedId } });
            },
            Delete(postId){
                apiClient.delete(`/Post/Delete/${postId}`)
                        .then(()=>{
                            this.message = "Xóa bài đăng thành công" ;
                            this.snackbar = true;
                            this.snackbarColor = 'green';
                            this.GetAllPost();
                        })
                        .catch(error=>{
                            this.message = "Xóa bài đăng lỗi: " + error.response?.data?.message || error.message;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            Found(postId){
                apiClient.put(`/Post/Found?postId=${postId}`)
                        .then(()=>{
                            this.message = "Hủy tìm người thành công" ;
                            this.snackbar = true;
                            this.snackbarColor = 'green';
                            this.GetAllPost();
                        })
                        .catch(error=>{
                            this.message = "Hủy tìm người lỗi: " + error.response?.data?.message || error.message;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            Repost(postId){
                apiClient.put(`/Post/Repost?postId=${postId}`)
                        .then(()=>{
                            this.message = "Đăng lại thành công" ;
                            this.snackbar = true;
                            this.snackbarColor = 'green';
                            this.GetAllPost();
                        })
                        .catch(error=>{
                            this.message = "Đăng lại lỗi: " + error.response?.data?.message || error.message;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            GotoDetail(postId){
                this.$router.resolve({ name: 'detailpost', params: { idpost: postId } });
            },
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
                <h1 v-if="postData == 0">Bạn không có bài đăng nào</h1>
                <v-col cols="12" md="8" v-else>
                    <h3>Danh sách các bài đăng của bạn</h3>
                    <BCard no-body v-for="(post,index) in postData" :key="index">
                        <BCardBody class="p-0">
                            <BRow>
                                <BCol class="col-md-4 col-12">
                                    <v-window style="height: 100%;"
                                        v-model="post.selectfile"
                                        show-arrows
                                        theme="dark"
                                    >
                                        <v-window-item
                                        v-for="(file,n) in post.pathImgRoom"
                                        :key="n"
                                        >
                                        <v-card class="d-flex justify-center align-center" height="200px">
                                            <v-img
                                                :src="file"
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
                                    <button to="/detail" class="text-left">
                                        <h4> {{ post.name }} </h4>
                                        <h5>Giá: <span style="color: red;">{{ post.price }}</span></h5>
                                        <p><v-icon>mdi-map-marker-radius</v-icon> {{ post.address }}</p>
                                        <p>Giới tính: {{ post.gender }}</p>
                                    </button>
                                    <div class="text-right">
                                        <BButton variant="white" @click="GotoDetail(post.id)"><v-icon size="24">mdi-eye</v-icon></BButton>
                                        <BButton variant="white" @click="Edit(post.id)"><v-icon size="24">mdi-pencil</v-icon></BButton>
                                        <BButton variant="white" @click="Delete(post.id)"><v-icon size="24">mdi-delete</v-icon></BButton>
                                        <BButton variant="white" v-if="post.status == false" @click="Found(post.id)"><v-icon size="24">mdi-account-multiple-remove</v-icon></BButton>
                                        <BButton variant="white" v-else @click="Repost(post.id)"><v-icon size="24">mdi-account-multiple-plus</v-icon></BButton>
                                    </div>
                                </BCol>
                            </BRow>
                        </BCardBody>
                    </BCard>
                </v-col>
            </v-row>
        </v-container>
</template>