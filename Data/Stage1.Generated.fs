namespace PhotoTour

/// Generated.
module AttractionCards =
    let ЗаповедникБасеги =
        { AttractionCard.Id = 15
          Name = "Заповедник Басеги"
          Place = "Пермский край"
          Kind = Kind.Natural
          Region = Region.Volga
          VictoryPoints = 2
          PhotoEquipments = [] }

    let СкульптураОлень =
        { AttractionCard.Id = 16
          Name = "Скульптура \"Олень\""
          Place = "Нижний Новгород"
          Kind = Kind.Urban
          Region = Region.Volga
          VictoryPoints = 1
          PhotoEquipments = [ Smartphone ] }

    let УтёсСтепанаРазина =
        { AttractionCard.Id = 17
          Name = "Утёс Степана Разина"
          Place = "Саратовская область"
          Kind = Kind.Natural
          Region = Region.Volga
          VictoryPoints = 4
          PhotoEquipments = [ Tripod; Smartphone ] }

    let НабережнаяБрюгге =
        { AttractionCard.Id = 18
          Name = "Набережная Брюгге"
          Place = "Йошкар-Ола"
          Kind = Kind.Urban
          Region = Region.Volga
          VictoryPoints = 1
          PhotoEquipments = [ Lens ] }

    let ЖигулёвскиеГоры =
        { AttractionCard.Id = 19
          Name = "Жигулёвские горы"
          Place = "Самарская область"
          Kind = Kind.Natural
          Region = Region.Volga
          VictoryPoints = 2
          PhotoEquipments = [ Smartphone; Quadcopter ] }

    let ДворецЗемледельцев =
        { AttractionCard.Id = 20
          Name = "Дворец земледельцев"
          Place = "Казань"
          Kind = Kind.Urban
          Region = Region.Volga
          VictoryPoints = 1
          PhotoEquipments = [ Quadcopter; Quadcopter ] }

    let ХребетЯлангас =
        { AttractionCard.Id = 21
          Name = "Хребет Ялангас"
          Place = "Республика Башкортостан"
          Kind = Kind.Natural
          Region = Region.Volga
          VictoryPoints = 3
          PhotoEquipments = [ Lens; Tripod; Smartphone ] }

    let all =
        [ ЗаповедникБасеги
          СкульптураОлень
          УтёсСтепанаРазина
          НабережнаяБрюгге
          ЖигулёвскиеГоры
          ДворецЗемледельцев
          ХребетЯлангас ]
