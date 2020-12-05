<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Solicitudes de afiliación</v-toolbar-title>
        <v-divider class="mx-2" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <v-text-field
          class="text-xs-center"
          v-model="search"
          append-icon="search"
          label="Búsqueda"
          single-line
          hide-details
        ></v-text-field>
        <v-spacer></v-spacer>
        <v-dialog v-model="adModal" max-width="290">
          <v-card>
            <!-- <v-card-title class="headline" v-if="adAccion == 1"
              >¿Activar Item?</v-card-title
            >
            <v-card-title class="headline" v-if="adAccion == 2"
              >¿Desactivar Item?</v-card-title
            > -->
            <v-card-text>
              Estás a punto de aprobar la solicitud con id:{{ id }}
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="green darken-1"
                flat="flat"
                @click="activarDesactivarCerrar"
              >
                Cancelar
              </v-btn>
              <v-btn color="orange darken-4" flat="flat" @click="guardar">
                Aprobar
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
      <v-data-table
        :headers="headers"
        :items="solicitudes"
        :search="search"
        class="elevation-1"
      >
        <template slot="items" slot-scope="props">
          <td class="justify-center layout px-0">
            <!-- <v-icon
                        small
                        class="mr-2"
                        @click="editItem(props.item)"
                        >
                        edit
                        </v-icon>
                        <v-icon
                        small
                        class="mr-2"
                        @click="deleteItem(props.item)"
                        >
                        delete
                        </v-icon> -->
            <!-- <template v-if="props.item.condicion">
                            <v-icon
                            small
                            @click="activarDesactivarMostrar(2,props.item)"
                            >
                            block
                            </v-icon>
                        </template>
                        <template v-else> -->
            <v-icon small @click="editItem(props.item)"> check </v-icon>
            <!-- </template> -->
          </td>
          <td>{{ props.item.idUsuario }}</td>
          <td>{{ props.item.fecha }}</td>
          <td>{{ props.item.referenciaBancariaUrl }}</td>
          <td>{{ props.item.reciboLuzUrl }}</td>
          <td>{{ props.item.reciboAguaUrl }}</td>
          <td>{{ props.item.reciboTelefonoUrl }}</td>
          <td>{{ props.item.pagareUrl }}</td>
          <td>{{ props.item.comentario }}</td>

          <!-- <td>
                        <div v-if="props.item.condicion">
                            <span class="blue--text">Activo</span>
                        </div>
                        <div v-else>
                            <span class="red--text">Inactivo</span>
                        </div>
                    </td> -->
        </template>
        <template slot="no-data">
          <v-btn color="primary" @click="listar">Resetear</v-btn>
        </template>
      </v-data-table>
    </v-flex>
  </v-layout>
</template>
<script>
    import axios from 'axios'
    export default {
        data(){
            return {
                solicitudes:[],                
                dialog: false,
                headers: [
                    { text: 'Opciones', value: 'opciones', sortable: false },
                    { text: 'Id del usuario', value: 'IdUsuario' },
                    { text: 'Fecha', value: 'fecha' },
                    { text: 'Ref. Bancaria', value: 'ReferenciaBancariaUrl', sortable: false },
                    { text: 'Recibo Luz', value: 'ReciboLuzUrl', sortable: false },
                    { text: 'Recibo Agua', value: 'ReciboAguaUrl', sortable: false },
                    { text: 'Recibo Telefonía', value: 'ReciboTelefonoUrl', sortable: false },
                    { text: 'Pagare', value: 'PagareUrl', sortable: false },
                    { text: 'Comentario', value: 'Comentario', sortable: false  },
                    // { text: 'Estado', value: 'condicion', sortable: false  }                
                ],
                search: '',
                editedIndex: -1,
                id: '',
                idUsuario: '',
                fecha: '',
                referenciaBancariaUrl: '',
                reciboLuzUrl:'',
                reciboAguaUrl: '',
                reciboTelefonoUrl: '',
                pagareUrl:'',
                comentario:'',
                estado: '',
                id_padre: null,
                valida: 0,
                validaMensaje:[],
                adModal: 0,
                deleteModal: 0,
                adAccion: 0,
                adNombre: '',
                adId: ''             
            }
        },
        computed: {
            formTitle () {
                return this.editedIndex === -1 ? 'Nueva categoría' : 'Actualizar categoría'
            }
        },

        watch: {
            dialog (val) {
            val || this.close()
            }
        },

        created () {
            this.listar();
        },
        methods:{
            listar(){
                let me=this;
                let header={"Authorization" : "Bearer " + this.$store.state.token};
                let configuracion= {headers : header};
                //console.log(configuracion);
                axios.get('api/SolicitudDeAfiliacion?estado=false',configuracion).then(function(response){
                    console.log(response.data);
                    me.solicitudes=response.data;
                }).catch(function(error){
                    console.log(error);
                });
            },
            editItem (item) {
                this.id = item.id,
                this.idUsuario = item.idUsuario,
                this.fecha= item.fecha,
                this.referenciaBancariaUrl= item.referenciaBancariaUrl,
                this.reciboLuzUrl= item.reciboLuzUrl,
                this.reciboAguaUrl= item.reciboAguaUrl,
                this.reciboTelefonoUrl= item.reciboTelefonoUrl,
                this.pagareUrl= item.pagareUrl,
                this.comentario= item.comentario,
                this.estado = "Aprobado",
                this.adModal = true
            },
            deleteItem (item) {
                this.id=item.id;
                this.nombre=item.nombre;
                this.id_padre=item.id_padre;
                this.editedIndex=1;
                this.deleteModal = true
            },

            eliminar () {
                //const index = this.desserts.indexOf(item)
                //this.id=item.id;
                // var r = confirm('Are you sure you want to delete this item?') //&& this.desserts.splice(index, 1)
                // if (r == true)
                // {
                    let header={"Authorization" : "Bearer " + this.$store.state.token};
                    let configuracion= {headers : header};
                    let me=this;
                    console.log('prueba')
                    axios.delete('api/Categorias/' + me.id,
                    configuracion).then(function(response){
                        me.close();
                        me.listar();
                        me.limpiar();                        
                    }).catch(function(error){
                        console.log(error);
                    });
                //}
            },

            close () {
                this.dialog = false;
                this.deleteModal = false;
                this.adModal = false;
                this.limpiar();
            },
            limpiar(){
                this.id= '',
                this.idUsuario= '',
                this.fecha= '',
                this.referenciaBancariaUrl= '',
                this.reciboLuzUrl='',
                this.reciboAguaUrl= '',
                this.reciboTelefonoUrl= '',
                this.pagareUrl='',
                this.comentario='',
                this.estado= '',
                this.editedIndex=-1;
            },
            guardar () {
                let header={"Authorization" : "Bearer " + this.$store.state.token};
                let configuracion= {headers : header};
                    //Código para editar
                    //Código para guardar
                    let me=this;
                    console.log('prueba')
                    axios.put('api/SolicitudDeAfiliacion/' + me.id,{
                        'id':me.id,
                        'idUsuario' : me.idUsuario,
                        'fecha': me.fecha,
                        'referenciaBancariaUrl': me.referenciaBancariaUrl,
                        'reciboLuzUrl': me.reciboLuzUrl,
                        'reciboAguaUrl': me.reciboAguaUrl,
                        'reciboTelefonoUrl' : me.reciboTelefonoUrl,
                        'pagareUrl' : me.pagareUrl,
                        'comentario': me.comentario,
                        'estado' : "Aprobado"
                    },configuracion).then(function(response){
                        me.close();
                        me.listar();
                        me.limpiar();                        
                    }).catch(function(error){
                        console.log(error);
                    });
                
            },
            validar(){
                this.valida=0;
                this.validaMensaje=[];

                if (this.nombre.length<3 || this.nombre.length>50){
                    this.validaMensaje.push("El nombre debe tener más de 3 caracteres y menos de 50 caracteres");
                }
                if (this.validaMensaje.length){
                    this.valida=1;
                }
                return this.valida;
            },
            activarDesactivarMostrar(accion,item){
                this.adModal=1;
                this.adNombre=item.nombre;
                this.adId=item.idcategoria;                
                if (accion==1){
                    this.adAccion=1;
                }
                else if (accion==2){
                    this.adAccion=2;
                }
                else{
                    this.adModal=0;
                }
            },
            activarDesactivarCerrar(){
                this.adModal=0;
            },
            activar(){
                let me=this;
                let header={"Authorization" : "Bearer " + this.$store.state.token};
                let configuracion= {headers : header};
                axios.put('api/Categorias/Activar/'+this.adId,{},configuracion).then(function(response){
                    me.adModal=0;
                    me.adAccion=0;
                    me.adNombre="";
                    me.adId="";
                    me.listar();                       
                }).catch(function(error){
                    console.log(error);
                });
            },
            desactivar(){
                let me=this;
                let header={"Authorization" : "Bearer " + this.$store.state.token};
                let configuracion= {headers : header};
                axios.put('api/Categorias/Desactivar/'+this.adId,{},configuracion).then(function(response){
                    me.adModal=0;
                    me.adAccion=0;
                    me.adNombre="";
                    me.adId="";
                    me.listar();                       
                }).catch(function(error){
                    console.log(error);
                });
            }
        }        
    }
</script>
