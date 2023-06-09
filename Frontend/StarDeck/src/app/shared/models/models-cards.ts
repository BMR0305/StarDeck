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

export interface Planet {
  id : any;
  p_name: any;
  p_effect : any;
  p_image: any;
  p_type: any;
  p_description: any;
  p_status: any;
}

export interface Deck {
  name : any;
  code: any;
  email_user : any;
  cards: any;
}

export interface Player {
  ID: any;
  avatar: any;
  birthday: any;
  coins: any;
  email: any;
  nationality: any;
  nickname: any;
  ranking: any;
  u_name: any;
  u_password: any;
  u_status: any;
  u_type: any;
}

export interface CardPlayed {

  GameId: any;
  CardId: any;
  PlayerId: any;
  Turn: any;
  Planet : any;

}


export interface CardPlayed_DTO {

  GameId: any;
  Card: Cards;
  PlayerId: any;
  Turn: any;
  Planet : any;

}
