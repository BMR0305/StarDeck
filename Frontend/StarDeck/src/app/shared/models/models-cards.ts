export interface Cards {
  id : any;
  c_name: any;
  battle_pts: any;
  energy : any;
  c_image: any;
  c_type: any;
  race : any;
  c_status: any;
  c_description: any;
}

export interface Deck {
  name : any;
  code : any;
  name_user : any;
  cards : Cards[];
}
