<template>
  <v-dialog v-model="dialog" persistent>
      <v-card>
        <v-card-title class="white--text light-blue darken-2">
          {{title}}
        </v-card-title>
        <v-card-text>
          <v-textarea v-model="input.message"
                      label="Ghi chú thêm"
          ></v-textarea>
        </v-card-text>
        <v-card-actions color="light-blue lighten-4">
          <v-btn color="success">
            Xác nhận
          </v-btn>
          <v-btn color="secondary">
            Hủy
          </v-btn>
        </v-card-actions>
      </v-card>
  </v-dialog>
</template>

<script>
  export default {
    name: "AddMessageDialog",
    props:[
      'title',
      'dialog'
    ],
    data(){
      return {
        input:{
          message: undefined
        }
      }
    },
    methods:{
        onConfirm(){
          this.$emit('confirm',this.input.message);
        }
    }
  }
</script>

<style scoped>

</style>
