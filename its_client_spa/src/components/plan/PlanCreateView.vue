<template>
  <v-content>
    <v-toolbar class="light-blue" flat dark>
      <v-toolbar-title>
        {{title}}
      </v-toolbar-title>
    </v-toolbar>
    <v-layout column mx-3>
      <v-flex my-3>
        <v-text-field
          label="Tên"
          v-model="input.name"
        />
        <AreaInput v-model="input.areaId">

        </AreaInput>
        <v-text-field
          label="Ngày bắt đầu"
          type="date"
          v-model="input.startDate"
        />
        <v-text-field
          label="Ngày kết thúc"
          type="date"
          v-model="input.endDate"
        />

      </v-flex>
      <v-flex>
        <v-btn color="success"
               :loading="createLoading"
               @click="onCreate">
          Tạo
        </v-btn>
        <v-btn color="secondary"
               @click="onCancel">
          Hủy
        </v-btn>
      </v-flex>
    </v-layout>
  </v-content>
</template>

<script>
  import {mapGetters} from "vuex";
import {AreaInput} from "../../common/input"
  export default {
    name: "PlanCreateView",
    components:{
      AreaInput
    },
    data() {
      return {
        createBtnLoading: false,
        input: {
          name: undefined,
          startDate: undefined,
          endDate: undefined,
          areaId: undefined,
        }
      }
    },
    computed: {
      title() {
        const {
          name
        } = this.$route;
        if (name === 'PlanCreate') {
          return "Tạo chuyến đi";
        } else {
          return "Chỉnh sửa chuyến đi";
        }
      },
      ...mapGetters('plan', {
        createLoading: 'createLoading',
      })
    },
    methods: {
      onCreate() {
        this.$store.dispatch('plan/create', {
          ...this.input
        })
          .then(value => {
            this.$router.back();
          })
      },
      onCancel() {
        this.$router.back();
      }
    }
  }
</script>

<style scoped>

</style>
